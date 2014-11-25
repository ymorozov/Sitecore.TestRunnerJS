(function () {
  console.log("SpeakTest suite was loaded.");

  var testsWasRun = false;
  var baseUrl = '/speaktest/assets/';
  require.config({
    paths: {
      mocha: baseUrl + 'mocha',
      chai: baseUrl + 'chai',
      sinon: baseUrl + 'sinon',
      fakeServer: baseUrl + 'fakeserver'
    },
  });

  require.config({ paths: { tests: '/testspeak/testfixture/getbyurl?url=' + window.location } });

  require(['require', 'chai', 'mocha', 'sinon'], function (require, chai) {
    expect = chai.expect;
    mocha.setup('bdd');

    require(['/-/speak/v1/assets/main.js'], function () {
      require(['tests', 'jquery'], function (tests, $) {
        if (!tests) {
          return;
        }

        $(document).ajaxStop(function () {
          console.log('Page loading completed.');

          if (!testsWasRun) {
            if (window.mochaPhantomJS) {
              mochaPhantomJS.run();
            } else {
              $('.sc-applicationContent').append($("<div id='mocha'/>"));
              mocha.run();
            }
            testsWasRun = true;
          }
        });
      });
    });
  });
})();