(function () {
  console.log("TestRunnerJS suite was loaded.");

  var baseUrl = '/sitecore/TestRunnerJS/';
  var assetsBaseUrl = baseUrl + 'assets/';
  var libsBaseUrl = baseUrl + 'libs/';

  var testFixtureExist = false;

  require.config({
    paths: {
      mocha: libsBaseUrl + 'mocha',
      chai: libsBaseUrl + 'chai',
      sinon: libsBaseUrl + 'sinon',
      fakeServer: assetsBaseUrl + 'fakeserver',
      loca: libsBaseUrl + 'loca'
    },
  });

  var xhr = new XMLHttpRequest();
  xhr.onreadystatechange = function () {
    if (xhr.readyState === 4) {
      if (xhr.status == 200) {
        var response = JSON.parse(xhr.responseText);
        if (response.IsExist) {
          require.config({ paths: { tests: response.ExpectedPath.split('.js')[0] } });
          testFixtureExist = true;
        } else {
          console.warn("Test fixture was not loaded. Expected path: " + response.ExpectedPath);
        }
      } else {
        console.warn("Test fixture was not loaded.");
      }
      loadEnvironment();
    }
  }

  xhr.open('GET', '/TestRunnerJS/testfixture/getbyurl?url=' + encodeURIComponent(window.location), true);
  xhr.setRequestHeader('Content-type', 'application/json');
  xhr.send();

  function loadEnvironment() {
    require(['require', 'chai', 'mocha', 'sinon'], function (require, chai) {
      window.expect = chai.expect;

      mocha.check = function (f, done) {
        return function () {
          try {
            f();
            done();
          } catch (e) {
            done(e);
          }
        }
      };

      mocha.setup('bdd');

      var taskId = 0,
        runnerCheckerIsStarted = false,
        lastBindingSeenTime = 0,
        bindingSeenTimeDelta = 0,
        testRunner = null;

      function runnerChecker() {
        bindingSeenTimeDelta = new Date().getTime() - lastBindingSeenTime;

        if (bindingSeenTimeDelta > 2000 && testRunner != null) {
          window.clearInterval(taskId);
          testRunner();
        }
      }

      function interceptConsole() {
        var console = window.console;
        if (!console) return;

        function intercept(method) {
          var original = console[method];
          console[method] = function () {
            var arg = arguments[0].toString();
            if (arg.lastIndexOf("Applying Binding for the element", 0) === 0
              || arg.lastIndexOf("Requiring files:", 0) === 0
              || arg.lastIndexOf("Requiring page code:", 0) === 0
              || arg.lastIndexOf("initialize -", 0) === 0) {

              lastBindingSeenTime = new Date().getTime();

              if (!runnerCheckerIsStarted) {
                taskId = window.setInterval(runnerChecker, 100);
                runnerCheckerIsStarted = true;
              }
            }

            if (original.apply) {
              // Do this for normal browsers
              original.apply(console, arguments);
            } else {
              // Do this for IE
              var message = Array.prototype.slice.apply(arguments).join(' ');
              original(message);
            }
          };
        }

        var methods = ['log', 'warn', 'error'];
        for (var i = 0; i < methods.length; i++)
          intercept(methods[i]);
      }

      interceptConsole();

      require(['/-/speak/v1/assets/main.js'], function () {
        if (testFixtureExist) {
          require(['tests', 'jquery'], function (tests, $) {
            if (!tests) {
              console.warn("Tests haven't been run.");
              console.info('In order to enable testing test fixture have to return "true" boolean value');
              return;
            }

            testRunner = function () {
              console.log('Running tests.');
              if (window.mochaPhantomJS) {
                mochaPhantomJS.stats = { fail: 0, pass: 0, total: 0 };
                mochaPhantomJS.run();
              } else {
                require(['loca'], function () {
                  mocha.reporter(mocha.WebKit);
                  mocha.run();
                });
              }
            };
          });
        }
      });
    });
  };
})();