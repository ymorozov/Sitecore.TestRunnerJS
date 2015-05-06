define(function () {
  require.config({ waitSeconds: 7 });

  mocha.ui('bdd');
  mocha.reporter('html');
  var should = chai.should();

  // ADD ANY TESTS HERE
  var tests = [
    '/tests/fixture.js'
  ];

  require(tests, function () {
    if (window.mochaPhantomJS) {
      mochaPhantomJS.run();
    } else {
      mocha.run();
    }
  });
});