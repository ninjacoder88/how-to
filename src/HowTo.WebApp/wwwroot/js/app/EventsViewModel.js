require(["knockout", "http", "pubsub", "jquery", "bootstrap"],
    function (ko, http, ps) {
        "use strict";

        Date.prototype.toYearMonthDayString = function () {
            const year = this.getFullYear();
            const month = this.getMonth() + 1;
            const day = this.getDate();
            return `${year}-${month.toString().padStart(2, "0")}-${day.toString().padStart(2, "0")}`;
        };

        Date.prototype.toHourMinuteString = function () {
            const hour = this.getHours();
            const minute = this.getMinutes();
            return `${hour.toString().padStart(2, "0")}:${minute.toString().padStart(2, "0")}`;
        };

        const apiUrl = document.getElementById("api-url").value;

        function HowToEvent(obj) {
            const self = this;
            self.title = obj.title;
            self.description = obj.description;
            self.startDate = obj.startDate;
            self.startTime = obj.startTime;
            self.endDate = obj.endDate;
            self.endTime = obj.endTime;
        }

        function CreateEvent() {
            const self = this;
            self.title = ko.observable("");
            self.description = ko.observable("");
            self.startDate = ko.observable("");
            self.startTime = ko.observable("");
            self.endDate = ko.observable("");
            self.endTime = ko.observable("");

            function reset() {
                const now = new Date();
                const later = new Date(now);
                later.setHours(later.getHours() + 1);
                self.title("");
                self.description("");
                self.startDate(now.toYearMonthDayString());
                self.startTime(now.toHourMinuteString());
                self.endDate(later.toYearMonthDayString());
                self.endTime(later.toHourMinuteString());
            }

            self.createEvent = function () {
                console.log(this);
                console.log(ko.toJSON(this));
                http.postRawAsync(`${apiUrl}api/events`, ko.toJSON(this))
                    .then(() => {
                        reset();
                        ps.publish("event-created");
                    }).catch(error => {
                        console.error(error);
                    });
            };

            reset();
        }

        function ViewModel() {
            const self = this;
            self.events = ko.observableArray([]);
            self.createEvent = new CreateEvent();

            ps.subscribe("event-created", () => {
                self.loadEvents();
            });

            self.loadEvents = function () {
                http.getAsync(`${apiUrl}api/events`)
                    .then(events => {
                        const array = [];
                        events.forEach(e => {
                            array.push(new HowToEvent(e));
                        });
                        self.events(array);
                    }).catch(error => {
                        console.error(error);
                    });
            };

            self.loadEvents();
        }

        ko.applyBindings(new ViewModel(), document.getElementById("app-container"));
    });