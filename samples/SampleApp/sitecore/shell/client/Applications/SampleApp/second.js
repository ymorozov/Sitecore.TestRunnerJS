define(["sitecore", "jquery"], function (_sc, $) {
  var dashboardDefinition = {
    initialized: function () {
      this.MyListDataSource.on("change:hasResponse", this.initializeList, this);
      this.MyListDataSource.refresh();
    },
    
    // Get data from response and pass it into list control and subscribe on selected item change
    initializeList: function() {
      var response = this.MyListDataSource.get("response");
      var items = JSON.parse(response);
      this.MyListControl.set("items", items);
      this.MyListControl.on("change:selectedItemId", this.showSelectedItem, this);
    },

    // Request data from the server on list control row selection and display response in ValueLabel
    showSelectedItem: function () {
      var selectedItem = this.MyListControl.get("selectedItem");
      if (selectedItem == "") {
        return;
      }
      
      var selectedItemName = selectedItem.get("Name");
      var url = "/test/SampleResponse/ProcessSelectedItem?selectedItem=" + selectedItemName;

      $.get(url, $.proxy(this.showResponse, this));
        
    },

    // Display response in ValueLabel
    showResponse: function (response) {
      this.ValueLabel.set("text", response);
    }
  };
  return _sc.Definitions.App.extend(dashboardDefinition);
});