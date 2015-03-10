(function () {
  // Define requirejs module for test fixture and inject sitecore dependency 
  define(['sitecore'], function (_sc) {
    
    // Define tests using mocha, sinon and chai
    describe('Another page', function () {
      it('Should load test from ActionTextBox into ResultTextBox on Action button click', function () {
        // arrange
        var expectedText = "Load into Result";
        // Set expected text into action text box
        _sc.app.ActionTextBox.set("text", expectedText);

        // act
        // Click ActionButton
        _sc.app.ActionButton.trigger("click");

        // assert
        // Ensure that ResultTextBox contains expected value
        expect(_sc.app.ResultTextBox.get("text")).to.equal(expectedText);
      });
    });

    // Return true to enable test running for this fixture, false or undefined will turn this test fixture off
    return true;
  });
})();