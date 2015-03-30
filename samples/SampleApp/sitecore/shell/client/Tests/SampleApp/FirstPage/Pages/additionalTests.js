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
      // Use mocha.check and pass assertion anonymous function and done callback
      _sc.app.ValueLabel.once("change:text", mocha.check(function () {
        var text = _sc.app.ValueLabel.get("text");
        expect(text).to.equal("Selected item \"My item 2\" was processed on server");
      }), done);
    });
  });

  // Return true to enable test running for this fixture, false or undefined will turn this test fixture off
  return true;
});
