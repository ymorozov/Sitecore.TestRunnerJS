define(["sitecore"], function (_sc) {
  var pageDefinition = {
    initialized: function () {
      this.ActionButton.on("click", this.doAction, this);
    },
    
    // Set text value from ActionTextBox to ResultTextBox on ActionButton click
    doAction: function() {
      var text = this.ActionTextBox.get("text");
      this.ResultTextBox.set("text", text);
    }
  };
  return _sc.Definitions.App.extend(pageDefinition);
});