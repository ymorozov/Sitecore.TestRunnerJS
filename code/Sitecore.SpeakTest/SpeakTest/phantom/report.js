/// <reference path="../assets/loca.js" />
var Base = require('./base')
  , cursor = Base.cursor
  , color = Base.color;

exports = module.exports = FAP;

function FAP(runner) {
  Base.call(this, runner);

  var self = this
    , stats = this.stats
    , n = 1
    , passes = 0
    , failures = 0;

  runner.on('start', function () {
    var total = runner.grepTotal(runner.suite);
    log('Total tests on page: ' + total);
  });

  runner.on('test end', function () {
    ++n;
  });

  runner.on('pending', function (test) {
    log('[SKIP]' + title(test));
  });

  runner.on('pass', function (test) {
    passes++;
    log('[PASS] ' + title(test));
  });

  runner.on('fail', function (test, err) {
    failures++;
    log('[FAIL] ' + title(test));
    if (err.stack) log(err.stack.replace(/^/gm, '  '));
  });

  runner.on('end', function () {
    mochaPhantomJS.stats.fail = failures;
    mochaPhantomJS.stats.pass = passes;
    mochaPhantomJS.stats.total = passes + failures;
  });
}

function title(test) {
  return test.fullTitle().replace(/#/g, '');
}

function log(msg) {
  console.log('#[TEST]#' + msg);
}