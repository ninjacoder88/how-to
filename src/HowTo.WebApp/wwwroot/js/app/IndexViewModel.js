require(["knockout", "http", "pubsub", "jquery", "bootstrap"],
    function (ko, http, ps) {
        "use strict";

        function HowToEvent(obj) {
            const self = this;
            self.title = obj.title;
            self.description = obj.description;
            self.startDate = obj.startDate;
            self.startTime = obj.startTime;
            self.endDate = obj.endDate;
            self.endTime = obj.endTime;
        }

        function ViewModel() {
            const self = this;
            const apiUrl = document.getElementById("api-url").value;

            self.loadEvents = function () {
                http.getAsync(`${apiUrl}api/events`)
                    .then(events => {
                        const array = [];
                        events.forEach(e => {
                            array.push(new HowToEvent(e));
                        });
                        ps.publish("events-loaded", { events: array });
                    }).catch(error => {
                        console.error(error);
                    });
            };

            self.loadEvents();
        }

        ko.components.register("calendar-component", {
            viewModel: { require: "components/calendar/calendar-component-view-model" },
            template: { require: "text!components/calendar/calendar-component.html" },
        });

        ko.applyBindings(new ViewModel(), document.getElementById("app-container"));
    });