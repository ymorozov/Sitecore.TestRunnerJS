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

  require.config({ paths: { tests: '/testspeak/testfixture/getbyurl?url=' + window.location } });

  require(['require', 'chai', 'mocha', 'sinon'], function (require, chai) {
    expect = chai.expect;
    mocha.setup('bdd');

    require(['/-/speak/v1/assets/main.js'], function () {
      require(['tests', 'jquery', 'loca'], function (tests, $) {
        if (!tests) {
          return;
        }

        var lastBindingSeenTime = 0;
        var bindingSeenTimeDelta = 0;
        var runnerIsStarted = false;

        var taskId = 0;

        function testRunner() {
          bindingSeenTimeDelta = new Date().getTime() - lastBindingSeenTime;
          
          if (bindingSeenTimeDelta > 2000) {
            window.clearInterval(taskId);
            
            console.log('Running tests.');

            if (window.mochaPhantomJS) {
              mochaPhantomJS.run();
            } else {
              mocha.reporter(mocha.WebKit);
              mocha.run();
            }
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

                if (!runnerIsStarted) {
                  taskId = window.setInterval(testRunner, 100);
                  runnerIsStarted = true;
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
      });
    });
  });
})();