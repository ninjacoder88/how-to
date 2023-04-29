require(["knockout", "jquery", "bootstrap"],
    function (ko) {
        "use strict";

        function ViewModel() {
            const self = this;
        }

        ko.components.register("calendar-component", {
            viewModel: { require: "components/calendar/calendar-component-view-model" },
            template: { require: "text!components/calendar/calendar-component.html" },
        });

        ko.applyBindings(new ViewModel(), document.getElementById("app-container"));
    });