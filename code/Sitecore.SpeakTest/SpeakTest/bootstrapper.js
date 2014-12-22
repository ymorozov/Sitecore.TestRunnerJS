(function () {
  console.log("SpeakTest suite was loaded.");

  var baseUrl = '/speaktest/assets/';
  require.config({
    paths: {
      mocha: baseUrl + 'mocha',
      chai: baseUrl + 'chai',
      sinon: baseUrl + 'sinon',
      fakeServer: baseUrl + 'fakeserver',
      loca: baseUrl + 'loca'
    },
  });

  require.config({ paths: { tests: '/testspeak/testfixture/getbyurl?url=' + encodeURIComponent(window.location) } });

  require(['require', 'chai', 'mocha', 'sinon'], function (require, chai) {
    expect = chai.expect;
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
      require(['tests', 'jquery'], function (tests, $) {
        if (!tests) {
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
    });
  });
})();