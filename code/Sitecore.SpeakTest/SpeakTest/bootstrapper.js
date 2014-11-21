(function () {
  console.log("SpeakTest suite was loaded.");

  var baseUrl = '/SpeakTest/assets';
  require.config({
    baseUrl: baseUrl,
    paths: {
      mocha: 'mocha',
      chai: 'chai',
      sinon: 'sinon',
      tests: 'tests'
    },
  });

  require(['require', 'chai', 'mocha', 'sinon'], function (require, chai) {
    expect = chai.expect;
    mocha.setup('bdd');

    require(['/-/speak/v1/assets/main.js'], function () {
    });
  });
})();