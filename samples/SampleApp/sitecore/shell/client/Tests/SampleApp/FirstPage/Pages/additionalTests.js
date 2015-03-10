(function () {
  // Define requirejs module for test fixture and inject sitecore and jquery dependency 
  define(['sitecore', 'jquery'], function (_sc, $) {

    // Define tests using mocha, sinon and chai
    describe('Second page', function () {
      it('Should process selected item', function (done) {
        // arrange
        // Get second row element
        var secondRow = $("td[title='My item 2']");

        // act
        // Click on second row
        secondRow.click();

        // assert
        // Asynchronously check text value on ValueLabel text change 
        _sc.app.ValueLabel.on("change:text", function() {
          var text = _sc.app.ValueLabel.get("text");
          expect(text).to.equal("Selected item \"My item 2\" was processed on server");
          // Tell mocha that async test is finished
          done();
        });
      });
    });

    // Return true to enable test running for this fixture, false or undefined will turn this test fixture off
    return true;
  });
})();