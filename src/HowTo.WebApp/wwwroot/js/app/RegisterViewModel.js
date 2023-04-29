require(["knockout", "http", "jquery", "bootstrap"],
    function (ko, http) {
        "use strict";

        function ViewModel() {
            const self = this;
            self.username = ko.observable("");
            self.password = ko.observable("");
            self.emailAddress = ko.observable("");
            self.registering = ko.observable(false);
            self.registrationFailed = ko.observable(false);
            self.errorMessage = ko.observable("");

            self.register = function () {
                self.registering(true);
                self.registrationFailed(false);
                self.errorMessage("");

                //todo: validate fields

                //todo: pull api location from configuration
                http.postAsync("http://localhost:8091/api/users", { username: self.username(), password: self.password(), emailAddress: self.emailAddress() })
                    .then(() => {
                        //todo: pull successful registration redirect from configuration
                        //todo: show user success message before redirecting
                        window.location = "/home/login";
                    }).catch(error => {
                        self.registrationFailed(true);
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