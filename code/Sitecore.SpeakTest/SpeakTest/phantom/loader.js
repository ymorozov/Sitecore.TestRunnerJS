﻿(function () {
  var Reporter, USAGE, config, fs, mocha, reporter, system, webpage,
    __bind = function (fn, me) { return function () { return fn.apply(me, arguments); }; };

  system = require('system');

  webpage = require('webpage');

  fs = require('fs');

  USAGE = "Usage: phantomjs mocha-phantomjs.coffee URL REPORTER [CONFIG]";

  Reporter = (function () {
    function Reporter(reporter, config, url) {
      this.reporter = reporter;
      this.config = config;
      this.checkStarted = __bind(this.checkStarted, this);
      this.waitForRunMocha = __bind(this.waitForRunMocha, this);
      this.waitForInitMocha = __bind(this.waitForInitMocha, this);
      this.waitForMocha = __bind(this.waitForMocha, this);
      this.url = url;
      this.columns = parseInt(system.env.COLUMNS || 75) * .75 | 0;
      this.mochaStartWait = this.config.timeout || 20000;
      this.startTime = Date.now();
      this.output = this.config.file ? fs.open(this.config.file, 'w') : system.stdout;
      if (!this.url) {
        this.fail(USAGE);
      }
    }

    Reporter.prototype.run = function (page) {
      this.page = page;
      this.initPage();
      return this.loadPage();
    };

    Reporter.prototype.customizeMocha = function (options) {
      return Mocha.reporters.Base.window.width = options.columns;
    };

    Reporter.prototype.customizeOptions = function () {
      return {
        columns: this.columns
      };
    };

    Reporter.prototype.fail = function (msg, errno) {
      if (this.output && this.config.file) {
        this.output.close();
      }
      if (msg) {
        console.log(msg);
      }
      return phantom.exit(errno || 1);
    };

    Reporter.prototype.finish = function () {
      if (this.config.file) {
        this.output.close();
      }
      return phantom.exit(this.page.evaluate(function () {
        return mochaPhantomJS.failures;
      }));
    };

    Reporter.prototype.initPage = function () {
      var cookie, _i, _len, _ref;
      this.page = webpage.create({
        settings: this.config.settings
      });
      if (this.config.headers) {
        this.page.customHeaders = this.config.headers;
      }
      _ref = this.config.cookies || [];
      for (_i = 0, _len = _ref.length; _i < _len; _i++) {
        cookie = _ref[_i];
        this.page.addCookie(cookie);
      }
      if (this.config.viewportSize) {
        this.page.viewportSize = this.config.viewportSize;
      }

      var verbose = this.config.verbose;
      this.page.onConsoleMessage = function (msg) {
        if (verbose) {
          return system.stdout.writeLine(msg);
        } else {
          var testLabel = "#[TEST]#";
          if (msg.substring(0, testLabel.length) === testLabel) {
            return system.stdout.writeLine(msg.substring(testLabel.length));
          }
        }
      };

      this.page.onResourceError = function (resErr) {
        return system.stdout.writeLine("Error loading resource " + resErr.url + " (" + resErr.errorCode + "). Details: " + resErr.errorString);
      };
      this.page.onError = (function (_this) {
        return function (msg, traces) {
          var file, index, line, _j, _len1, _ref1;
          if (_this.page.evaluate(function () {
            return window.onerror != null;
          })) {
            return;
          }
          for (index = _j = 0, _len1 = traces.length; _j < _len1; index = ++_j) {
            _ref1 = traces[index], line = _ref1.line, file = _ref1.file;
            traces[index] = "  " + file + ":" + line;
          }
          return _this.fail("" + msg + "\n\n" + (traces.join('\n')));
        };
      })(this);
      return this.page.onInitialized = (function (_this) {
        return function () {
          return _this.page.evaluate(function (env) {
            return window.mochaPhantomJS = {
              env: env,
              failures: 0,
              ended: false,
              started: false,
              run: function () {
                mochaPhantomJS.started = true;
                window.callPhantom({
                  'mochaPhantomJS.run': true
                });
                return mochaPhantomJS.runner;
              }
            };
          }, system.env);
        };
      })(this);
    };

    Reporter.prototype.loadPage = function () {
      this.page.open(this.url);
      this.page.onLoadFinished = (function (_this) {
        return function (status) {
          _this.page.onLoadFinished = function () { };
          if (status !== 'success') {
            _this.onLoadFailed();
          }
          return _this.waitForInitMocha();
        };
      })(this);
      return this.page.onCallback = (function (_this) {
        return function (data) {
          if (data != null ? data.hasOwnProperty('Mocha.process.stdout.write') : void 0) {
            _this.output.write(data['Mocha.process.stdout.write']);
          } else if (data != null ? data.hasOwnProperty('mochaPhantomJS.run') : void 0) {
            if (_this.injectJS()) {
              _this.waitForRunMocha();
            }
          }
          return true;
        };
      })(this);
    };

    Reporter.prototype.onLoadFailed = function () {
      return this.fail("Failed to load the page. Check the url: " + this.url);
    };

    Reporter.prototype.injectJS = function () {
      if (this.page.evaluate(function () {
        return window.mocha != null;
      })) {
        this.page.injectJs('mocha-phantomjs/core_extensions.js');
        this.page.evaluate(this.customizeMocha, this.customizeOptions());
        return true;
      } else {
        this.fail("Failed to find mocha on the page.");
        return false;
      }
    };

    Reporter.prototype.runMocha = function () {
      var customReporter, wrappedReporter, wrapper, _base;
      if (this.config.useColors === false) {
        this.page.evaluate(function () {
          return mocha.useColors(false);
        });
      }
      if (typeof (_base = this.config.hooks).beforeStart === "function") {
        _base.beforeStart(this);
      }
      if (this.page.evaluate(this.setupReporter, this.reporter) !== true) {
        customReporter = fs.read(this.reporter);
        wrapper = function () {
          var exports, module, process, require;
          require = function (what) {
            var r;
            what = what.replace(/[^a-zA-Z0-9]/g, '');
            for (r in Mocha.reporters) {
              if (r.toLowerCase() === what) {
                return Mocha.reporters[r];
              }
            }
            throw new Error("Your custom reporter tried to require '" + what + "', but Mocha is not running in Node.js in mocha-phantomjs, so Node modules cannot be required - only other reporters");
          };
          module = {};
          exports = void 0;
          process = Mocha.process;
          'customreporter';
          return Mocha.reporters.Custom = exports || module.exports;
        };
        wrappedReporter = wrapper.toString().replace("'customreporter'", "(function() {" + (customReporter.toString()) + "})()");
        this.page.evaluate(wrappedReporter);
        if (this.page.evaluate(function () {
          return !Mocha.reporters.Custom;
        }) || this.page.evaluate(this.setupReporter) !== true) {
          this.fail("Failed to use load and use the custom reporter " + this.reporter);
        }
      }
      if (this.page.evaluate(this.runner)) {
        this.mochaRunAt = new Date().getTime();
        return this.waitForMocha();
      } else {
        return this.fail("Failed to start mocha.");
      }
    };

    Reporter.prototype.waitForMocha = function () {
      var ended, _base;
      ended = this.page.evaluate(function () {
        return mochaPhantomJS.ended;
      });
      if (ended) {
        if (typeof (_base = this.config.hooks).afterEnd === "function") {
          _base.afterEnd(this);
        }
        return this.finish();
      } else {
        return setTimeout(this.waitForMocha, 100);
      }
    };

    Reporter.prototype.waitForInitMocha = function () {
      if (!this.checkStarted()) {
        return setTimeout(this.waitForInitMocha, 100);
      }
    };

    Reporter.prototype.waitForRunMocha = function () {
      if (this.checkStarted()) {
        return this.runMocha();
      } else {
        return setTimeout(this.waitForRunMocha, 100);
      }
    };

    Reporter.prototype.checkStarted = function () {
      var started;
      started = this.page.evaluate(function () {
        return mochaPhantomJS.started;
      });
      if (!started && this.mochaStartWait && this.startTime + this.mochaStartWait < Date.now()) {
        this.fail("Failed to start mocha: Init timeout", 255);
      }
      return started;
    };

    Reporter.prototype.setupReporter = function (reporter) {
      var error;
      try {
        mocha.setup({
          reporter: reporter || Mocha.reporters.Custom
        });
        return true;
      } catch (_error) {
        error = _error;
        return error;
      }
    };

    Reporter.prototype.runner = function () {
      var cleanup, error, _ref, _ref1;
      try {
        mochaPhantomJS.runner = mocha.run();
        if (mochaPhantomJS.runner) {
          cleanup = function () {
            mochaPhantomJS.failures = mochaPhantomJS.runner.failures;
            return mochaPhantomJS.ended = true;
          };
          if ((_ref = mochaPhantomJS.runner) != null ? (_ref1 = _ref.stats) != null ? _ref1.end : void 0 : void 0) {
            cleanup();
          } else {
            mochaPhantomJS.runner.on('end', cleanup);
          }
        }
        return !!mochaPhantomJS.runner;
      } catch (_error) {
        error = _error;
        return false;
      }
    };

    return Reporter;

  })();

  var testPages = null;
  var instanceName = system.args[1];
  var applicationName = system.args[2];

  var settingsPage = webpage.create();
  settingsPage.open("http://" + instanceName + "/speaktest/phantom/settings.html?app=" + applicationName, function () {
    console.log('Loading test settings.');

    testPages = settingsPage.evaluate(function () {
      return testPages;
    });

    if (testPages) {
      console.log('Test settings was loaded.');
      loginIntoSitecore(testPages);
    } else {
      console.log('Error loading test settings. Test execution terminated!');
      phantom.exit(-1);
    }
  });

  function loginIntoSitecore(testPages) {
    var loginPageUrl = "http://" + instanceName + "/sitecore/login";
    var loginPage = webpage.create();
    loginPage.open(loginPageUrl, "post", { n: "" }, function () {
      console.log('Login page loaded! Url: ' + loginPage.url);

      loginPage.onLoadFinished = function () {
        console.log('Launch pad page loaded! Url: ' + loginPage.url);

        beginTestExecution(loginPage.cookies, testPages);
      };

      loginPage.evaluate(function () {
        $("#UserName").val("admin");
        $("#Password").val("b");
        $(".btn-primary").click();
      });
    });
  }

  function beginTestExecution(loginCookies, testPages) {
    if (phantom.version.major < 1 || (phantom.version.major === 1 && phantom.version.minor < 9)) {
      console.log('mocha-phantomjs requires PhantomJS > 1.9.1');
      phantom.exit(-1);
    }

    reporter = 'report.js';

    config = JSON.parse(system.args[3] || '{}');
    config.cookies = loginCookies;

    config.verbose = false;

    if (config.hooks) {
      config.hooks = require(config.hooks);
    } else {
      config.hooks = {};
    }

    for (var i = 0; i < testPages.length; i++) {
      var testPage = "http://" + instanceName + testPages[i];
      console.log('Testing page: ' + testPage);

      mocha = new Reporter(reporter, config, testPage);
      mocha.run();
    }
  }
}).call(this);
