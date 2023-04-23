require(["knockout", "httpService", "pubsub", "bootstrap", "jquery"],
    function (ko, http, ps) {
        "use strict";

        function Token(obj) {
            const self = this;
            self.accessToken = obj.access_token;
            self.expiresIn = obj.expires_in;
            self.scope = obj.scope;
        }

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
            self.username = ko.observable("testaccount");
            self.password = ko.observable("password1234");
            self.clientId = ko.observable("test");
            self.clientSecret = ko.observable("ee09b556-a7ab-40cf-9a14-cd9e629a6bb7");
            self.tokenResponse = ko.observable();

            self.customerId = ko.observable("644543adae453d159183a104");
            self.customer = ko.observable();

            self.createCustomer = ko.observable(new CreateCustomer());

            self.loadingToken = ko.observable(false);
            self.tokenLoaded = ko.observable(false);

            self.getToken = function () {
                self.loadingToken(true);
                const queryString = `username=${self.username()}&password=${self.password()}&clientId=${self.clientId()}&clientSecret=${self.clientSecret()}`;
                http.getAsync(`http://localhost:8091/api/authenticate?${queryString}`)
                    .then(response => {
                        const token = new Token(response);
                        window.accessToken = token.accessToken;
                        //self.tokenResponse(new Token(response));
                        //self.tokenResponseText(response);
                        self.loadingToken(false);
                        self.tokenLoaded(true);
                    }).catch(error => {
                        console.error(error);
                        self.loadingToken(false);
                    });
            };

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

        ko.applyBindings(new ViewModel(), document.getElementById("container"));
    });