define(["knockout", "pubsub", "bootstrap", "jquery"],
    function (ko, ps) {
        "use strict";

        ko.bindingHandlers.singleClick = {
            init: function (element, valueAccessor) {
                const handler = valueAccessor();
                const delay = 200;
                let clickTimeout = false;

                $(element).click(function () {
                    if (clickTimeout !== false) {
                        clearTimeout(clickTimeout);
                        clickTimeout = false;
                    } else {
                        clickTimeout = setTimeout(function () {
                            clickTimeout = false;
                            handler();
                        }, delay);
                    }
                });
            }
        };

        function CalendarEvent(obj) {
            const self = this;
            self.eventId = obj.eventId;
            self.title = obj.title;

            self.showEvent = function () {
                ps.publish("event-clicked", { calendarEvent: obj });
            };
        }

        function CalendarDay(obj) {
            const self = this;
            self.dayOfMonth = obj.dayOfMonth;
            self.month = obj.month + 1;
            self.year = obj.year;
            self.displayDate = `${self.year}-${self.month.toString().padStart(2, "0")}-${self.dayOfMonth.toString().padStart(2, "0")}`;
            self.events = ko.observableArray([]);
        }

        function CalendarWeek(obj) {
            const self = this;
            self.weekNumber = obj.weekNumber;
            self.days = [];
        }

        return function () {
            const self = this;
            const now = new Date();
            const monthNames = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];

            self.calendarVisible = ko.observable(true);
            self.calendarMonth = ko.observable(now.getMonth());
            self.calendarYear = ko.observable(now.getFullYear());
            self.weeks = ko.observableArray([]);
            self.monthYear = ko.computed(function () {
                return `${monthNames[self.calendarMonth()]} ${self.calendarYear()}`;
            });
            let days = [];
            self.events = ko.observableArray([]);

            ps.subscribe("events-loaded", (message) => {
                console.log(message);
                self.events(message.events);
                updateWeeks();
            });

            function getDaysInMonth(month, year) {
                switch (month) {
                    case 3://april
                    case 5://june
                    case 8://september
                    case 10://november
                        return 30;
                    case 1://february
                        if (year % 4 === 0) {
                            if (year % 100 === 0) {
                                if (year % 400 === 0) {
                                    return 29;
                                }
                                return 28;
                            }
                            return 29;
                        }
                        return 28;
                    default:
                        return 31;
                }
            }

            function getWeeksInMonth(daysInMonth, startDay) {
                switch (daysInMonth) {
                    case 28:
                        if (startDay === 0) {
                            return 4;
                        }
                        return 5;
                    case 29:
                        return 5;
                    case 30:
                        if (startDay === 6) {
                            return 6;
                        }
                        return 5;
                    case 31:
                        if (startDay === 5 || startDay === 6) {
                            return 6;
                        }
                        return 5;
                }
            }

            function updateWeeks() {
                const firstOfMonth = new Date(self.calendarYear(), self.calendarMonth(), 1);
                const firstDayOfMonth = firstOfMonth.getDay();
                const daysInMonth = getDaysInMonth(self.calendarMonth(), self.calendarYear());
                const weeksInMonth = getWeeksInMonth(daysInMonth, firstDayOfMonth);

                self.weeks([]);
                days = [];
                let day = 1;
                for (var w = 0; w < weeksInMonth; w++) {
                    const calendarWeek = new CalendarWeek({weekNumber: w+1});
                    for (var d = 0; d < 7; d++) {
                        if (w === 0 && d < firstDayOfMonth) {
                            calendarWeek.days.push(new CalendarDay({ dayOfMonth: "", month: self.calendarMonth(), year: self.calendarYear() }));
                        } else {
                            if (day > daysInMonth) {
                                const d = new CalendarDay({ dayOfMonth: "", month: self.calendarMonth(), year: self.calendarYear() });
                                days.push(d);
                                calendarWeek.days.push(d);
                            } else {
                                const d = new CalendarDay({ dayOfMonth: day++, month: self.calendarMonth(), year: self.calendarYear() });
                                days.push(d);
                                calendarWeek.days.push(d);
                            }
                        }
                    }
                    self.weeks.push(calendarWeek);
                }

                self.events().forEach(e => {
                    days.forEach(d => {
                        if (e.startDate === d.displayDate) {
                            d.events.push(new CalendarEvent(e));
                        }
                    });
                });
            }

            self.initialize = function () {
                updateWeeks();
            };

            self.nextMonth = function () {
                const current = self.calendarMonth();
                if (current === 11) {
                    self.calendarMonth(0);
                    self.calendarYear(self.calendarYear() + 1);
                    updateWeeks();
                    return;
                }
                self.calendarMonth(current + 1);
                updateWeeks();
            };

            self.previousMonth = function () {
                const current = self.calendarMonth();
                if (current === 0) {
                    self.calendarMonth(11);
                    self.calendarYear(self.calendarYear() - 1);
                    updateWeeks();
                    return;
                }
                self.calendarMonth(current - 1);
                updateWeeks();
            };

            self.initialize();
        };
    });