require(["knockout", "httpService", "bootstrap", "pubsub", "jquery"],
    function (ko, http, bs, ps) {
        "use strict";

        function ViewModel() {
            const self = this;
        }

        ko.applyBindings(new ViewModel(), document.getElementById("container"));
    });