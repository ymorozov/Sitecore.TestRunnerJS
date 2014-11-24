(function () {
  console.log("SpeakTest suite was loaded.");

  var baseUrl = '/SpeakTest/assets';
  require.config({
    baseUrl: baseUrl,
    paths: {
      mocha: 'mocha',
      chai: 'chai',
      sinon: 'sinon',
      fakeServer: 'fakeserver'
    },
  });

  require.config({ paths: { tests: '/testspeak/testfixture/getbyurl?url=' + window.location } });

  require(['require', 'chai', 'mocha', 'sinon', 'tests'], function (require, chai) {
    expect = chai.expect;
    mocha.setup('bdd');

    require(['/-/speak/v1/assets/main.js'], function () {
    });
  });
})();