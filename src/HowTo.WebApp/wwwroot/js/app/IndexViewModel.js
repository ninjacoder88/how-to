require(["knockout", "http", "jquery", "bootstrap"],
    function (ko, http, $) {
        "use strict";

        function Customer(obj) {
            const self = this;
            self.customerId = obj.customerId;
            self.firstName = obj.firstName;
            self.lastName = obj.lastName;
            self.emailAddress = obj.emailAddress;
        }

        function CreateCustomer() {
            const self = this;
            self.firstName = ko.observable("");
            self.lastName = ko.observable("");
            self.emailAddress = ko.observable("");
            self.billingStreetAddress = ko.observable("");
            self.billingCity = ko.observable("");
            self.billingState = ko.observable("");
            self.billingPostalCode = ko.observable("");
            self.billingCountry = ko.observable("");
            self.shippingStreetAddress = ko.observable("");
            self.shippingCity = ko.observable("");
            self.shippingState = ko.observable("");
            self.shippingPostalCode = ko.observable("");
            self.shippingCountry = ko.observable("");

            function clear() {
                self.firstName("");
                self.lastName("");
                self.emailAddress("");
                self.billingStreetAddress("");
                self.billingCity("");
                self.billingState("");
                self.billingPostalCode("");
                self.billingCountry("");
                self.shippingStreetAddress("");
                self.shippingCity("");
                self.shippingState("");
                self.shippingPostalCode("");
                self.shippingCountry("");
            }

            self.createCustomer = function () {
                http.postRawAsync("http://localhost:8091/api/customer", ko.toJSON(self))
                    .then(response => {
                        console.log(response);
                        clear();
                    }).catch(error => {
                        console.error(error);
                    });
            };
        }

        function ViewModel() {
            const self = this;
            self.customerId = ko.observable("644543adae453d159183a104");
            self.customer = ko.observable();

            self.createCustomer = ko.observable(new CreateCustomer());

            self.getCustomer = function () {
                http.getAsync(`http://localhost:8091/api/customer/${self.customerId()}`)
                    .then(response => {
                        //self.customerResponseText(response);
                        self.customer(new Customer(response));
                    }).catch(error => {
                        console.error(error);
                    });
            };
        }

        ko.components.register("calendar-component", {
            viewModel: { require: "components/calendar/calendar-component-view-model" },
            template: { require: "text!components/calendar/calendar-component.html" },
        });

        ko.applyBindings(new ViewModel(), document.getElementById("app-container"));
    });