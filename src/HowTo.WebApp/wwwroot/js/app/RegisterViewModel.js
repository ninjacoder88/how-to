require(["knockout", "http", "jquery", "bootstrap"],
    function (ko, http) {
        "use strict";

        function ViewModel() {
            const self = this;
            const apiUrl = document.getElementById("api-url").value;
            const successUrl = document.getElementById("success-url").value;
            self.username = ko.observable("");
            self.password = ko.observable("");
            self.emailAddress = ko.observable("");
            self.registering = ko.observable(false);
            self.errorMessage = ko.observable("");
            self.registrationSuccess = ko.observable(false);
            self.registrationFailed = ko.computed(function () {
                return self.errorMessage() !== "";
            });

            self.register = function () {
                self.registering(true);
                self.errorMessage("");

                if (self.username().length < 5) {
                    self.errorMessage("Username must be at least 5 characters");
                    self.registering(false);
                    return;
                }

                if (self.password().length < 8) {
                    self.errorMessage("Password must be at least 8 characters");
                    self.registering(false);
                    return;
                }

                if (self.emailAddress().length < 8) {
                    self.errorMessage("Email address is invalid");
                    self.registering(false);
                    return;
                }

                http.postAsync(`${apiUrl}api/users`, { username: self.username(), password: self.password(), emailAddress: self.emailAddress() })
                    .then(() => {
                        self.registrationSuccess(true);
                        window.setTimeout(() => {
                            window.location = successUrl;
                        }, 5000);
                    }).catch(error => {
                        self.errorMessage(error);
                        console.error(error);
                    }).always(() => {
                        self.registering(false);
                    });
            };

            document.getElementById("passwordInput")
                .addEventListener("keyup", (e) => {
                    if (e.key === "Enter") {
                        self.register();
                    }
                });

            document.getElementById("usernameInput")
                .addEventListener("keyup", (e) => {
                    if (e.key === "Enter") {
                        self.register();
                    }
                });

            document.getElementById("emailInput")
                .addEventListener("keyup", (e) => {
                    if (e.key === "Enter") {
                        self.register();
                    }
                });
        }

        ko.applyBindings(new ViewModel(), document.getElementById("app-container"));
    });