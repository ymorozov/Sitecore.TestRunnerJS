define(["/sitecore/testrunnerjs/phantom/inputparams.js"], function (inputParams) {
  describe('Input parameters', function () {
    it('Should parse first parameter as instance name', function () {
      // arrange
      var args = ["-", "instance_name"];

      // act
      var result = inputParams.parse(args);

      // assert
      result.instanceName.should.equal("instance_name");
    });

    it('Should parse second parameter as application name', function () {
      // arrange
      var args = ["-", "-", "application_name"];

      // act
      var result = inputParams.parse(args);

      // assert
      result.applicationName.should.equal("application_name");
    });
  });
});