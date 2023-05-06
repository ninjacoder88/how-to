require(["knockout", "http", "pubsub", "jquery", "bootstrap"],
    function (ko, http, ps) {
        "use strict";

        function HowToEvent(obj) {
            const self = this;
            self.eventId = obj.eventId;
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
            self.currentEvent = ko.observable();

            ps.subscribe("event-clicked", (message) => {
                self.currentEvent(message.calendarEvent);
            });

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
            viewModel: { require: "components/calendar-component/calendar-component-view-model" },
            template: { require: "text!components/calendar-component/calendar-component.html" },
        });

        ko.applyBindings(new ViewModel(), document.getElementById("app-container"));
    });