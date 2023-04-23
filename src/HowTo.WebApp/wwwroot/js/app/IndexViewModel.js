require(["knockout", "httpService", "bootstrap", "pubsub", "jquery"],
    function (ko, http, bs, ps, $) {
        "use strict";

        function TokenResponse(obj) {
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

        function ViewModel() {
            const self = this;
            self.username = ko.observable("testuser");
            self.password = ko.observable("pword1234");
            self.clientId = ko.observable("test");
            self.clientSecret = ko.observable("ee09b556-a7ab-40cf-9a14-cd9e629a6bb7");
            self.tokenResponseText = ko.observable("");
            self.tokenResponse = ko.observable();

            self.customerId = ko.observable("644543adae453d159183a104");
            self.customerResponseText = ko.observable("");
            self.customer = ko.observable();

            self.getToken = function () {
                const queryString = `username=${self.username()}&password=${self.password()}&clientId=${self.clientId()}&clientSecret=${self.clientSecret()}`;
                $.ajax({
                    method: "GET",
                    url: "http://localhost:8091/api/authenticate?" + queryString,
                }).then(response => {
                    console.log(response);
                    self.tokenResponse(new TokenResponse($.parseJSON(response)));
                    self.tokenResponseText(response);
                }).catch(error => {
                    console.error(error);
                });
            };

            self.getCustomer = function () {
                http.getAsync(`http://localhost:8091/api/customer/${self.customerId()}`, self.tokenResponse().accessToken)
                    .then(response => {
                        self.customerResponseText(response);
                        self.customer(new Customer(response.data));
                    }).catch(error => {
                        console.error(error);
                    });
            };
        }

        ko.applyBindings(new ViewModel(), document.getElementById("container"));
    });