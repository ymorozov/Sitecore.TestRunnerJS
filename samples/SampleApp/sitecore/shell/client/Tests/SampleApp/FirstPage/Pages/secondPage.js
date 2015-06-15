// Define requirejs module for test fixture and inject sitecore and fakeServer dependency
// Fake server will override all data requests to the server
define(['fakeServer', 'sitecore'], function (server, _sc) {
  // Setup fake server response to load items into the list control during page init in test fixture global scope.
  server.respondWith(
    // Request type
    "POST",
    // Request URL
    "/test/SampleResponse/GetItems",
    // Response code, HTTP headers and content
    [200, { "Content-Type": "application/json" }, JSON.stringify(
      // Content contains JSON with 3 items
      [
        {
          "Name": "Test item 1",
          "Value": "Test value 1"
        }, {
          "Name": "Test item 2",
          "Value": "Test value 2"
        }, {
          "Name": "Test item 3",
          "Value": "Test value 3"
        }]
    )]);

  // Define tests using mocha, sinon and chai
  describe('Second page', function () {
    it('Should load list data from server', function () {
      // assert
      // Ensure that list control contains expected items with expected values
      var items = _sc.app.MyListControl.get("items");
      expect(items[0].Name).to.equal("Test item 1");
      expect(items[1].Name).to.equal("Test item 2");
      expect(items[2].Name).to.equal("Test item 3");
      expect(items[0].Value).to.equal("Test value 1");
      expect(items[1].Value).to.equal("Test value 2");
      expect(items[2].Value).to.equal("Test value 3");
    });

    it('Should show message from server on item selection', function (done) {
      // arrange
      var expectedResponse = "Test item 3 was processed on fake server";
      // Setup fake server to respond on request dureng intem selection on list control
      server.respondWith(
        // Request type
        "GET",
        // Request URL
        "/test/SampleResponse/ProcessSelectedItem?selectedItem=Test item 3",
        // Response code, HTTP header and content
        [200, { "Content-Type": "application/text" }, expectedResponse]
      );

      // Get third row on list control
      var thirdRow = $("td[title='Test item 3']");

      // act
      // Click on third row
      thirdRow.click();

      // assert
      // Asynchronously check text value on ValueLabel text change 
      // Use mocha.check and pass assertion anonymous function and done callback
      _sc.app.ValueLabel.once("change:text", mocha.check(function () {
        var text = _sc.app.ValueLabel.get("text");
        expect(text).to.equal(expectedResponse);
      }, done));
    });
  });

  // Return true to enable test running for this fixture, false or undefined will turn this test fixture off
  return true;
});
