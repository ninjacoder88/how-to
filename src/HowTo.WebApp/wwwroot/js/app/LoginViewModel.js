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
            self.username = ko.observable("");
            self.password = ko.observable("");
            self.loggingIn = ko.observable(false);
            self.errorMessage = ko.observable("");
            self.loginFailed = ko.observable(false);

            self.login = function () {
                self.loggingIn(true);
                self.loginFailed(false);
                self.errorMessage("");

                //todo: validate fields

                //todo: pull api location from configuration
                http.postAsync("http://localhost:8091/api/users/login", { username: self.username(), password: self.password() })
                    .then(response => {
                        const token = new Token(response);
                        window.accessToken = token.accessToken;
                        //todo: pull successful login redirect from configuration
                        window.location = "/events";
                    }).catch(error => {
                        self.loginFailed(true);
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