require(["knockout", "http", "jquery", "bootstrap"],
    function (ko, http) {
        "use strict";

        function Token(obj) {
            const self = this;
            self.accessToken = obj.access_token;
            self.expiresIn = obj.expires_in;
            self.scope = obj.scope;
        }

        function ViewModel() {
            const self = this;
            const apiUrl = document.getElementById("api-url").value;
            const successUrl = document.getElementById("success-url").value;
            self.username = ko.observable("");
            self.password = ko.observable("");
            self.loggingIn = ko.observable(false);
            self.errorMessage = ko.observable("");
            self.loginFailed = ko.computed(function () {
                return self.errorMessage() !== "";
            });

            self.login = function () {
                self.loggingIn(true);
                self.errorMessage("");

                if (self.username().length < 5) {
                    self.errorMessage("Username must be at least 5 characters");
                    self.loggingIn(false);
                    return;
                }

                if (self.password().length < 8) {
                    self.errorMessage("Password must be at least 8 characters");
                    self.loggingIn(false);
                    return;
                }

                http.postAsync(`${apiUrl}api/users/login`, { username: self.username(), password: self.password() })
                    .then(response => {
                        const token = new Token(response);
                        window.accessToken = token.accessToken;
                        window.location = successUrl;
                    }).catch(error => {
                        self.errorMessage(error);
                        console.error(error);
                    }).always(() => {
                        self.loggingIn(false);
                    });
            };

            document.getElementById("passwordInput")
                .addEventListener("keyup", (e) => {
                    if (e.key === "Enter") {
                        self.login();
                    }
                });

            document.getElementById("usernameInput")
                .addEventListener("keyup", (e) => {
                    if (e.key === "Enter") {
                        self.login();
                    }
                });
        }

        ko.applyBindings(new ViewModel(), document.getElementById("app-container"));
    });