﻿define(["knockout", "bootstrap", "jquery"],
    function (ko) {
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

        function CalendarDay(obj) {
            const self = this;
            self.dayOfMonth = obj.dayOfMonth;
            self.month = obj.month + 1;
            self.year = obj.year;
            self.displayDate = `${self.year.toString().padStart(2, "0")}-${self.month.toString().padStart(2, "0")}-${self.dayOfMonth.toString().padStart(2, "0")}`;

            self.showDay = function () {
                if (self.dayOfMonth === "") {
                    return;
                }
                window.alert(self.displayDate);
            };

            self.addEvent = function () {
                if (self.dayOfMonth === "") {
                    return;
                }
                window.confirm("Are you sure you want to add?");
            };
        }

        function CalendarWeek(obj) {
            const self = this;
            self.weekNumber = obj.weekNumber;
            self.days = [];
        }

        return function (params) {
            const self = this;
            const now = new Date();
            const monthNames = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
            const injected = params;

            self.calendarVisible = ko.observable(true);
            self.calendarMonth = ko.observable(now.getMonth());
            self.calendarYear = ko.observable(now.getFullYear());
            self.weeks = ko.observableArray([]);
            self.monthYear = ko.computed(function () {
                return `${monthNames[self.calendarMonth()]} ${self.calendarYear()}`;
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
                let day = 1;
                for (var w = 0; w < weeksInMonth; w++) {
                    const calendarWeek = new CalendarWeek({weekNumber: w+1});
                    //const days = [];
                    for (var d = 0; d < 7; d++) {
                        if (w === 0 && d < firstDayOfMonth) {
                            calendarWeek.days.push(new CalendarDay({ dayOfMonth: "", month: self.calendarMonth(), year: self.calendarYear() }));
                        } else {
                            if (day > daysInMonth) {
                                calendarWeek.days.push(new CalendarDay({ dayOfMonth: "", month: self.calendarMonth(), year: self.calendarYear() }));
                            } else {
                                calendarWeek.days.push(new CalendarDay({ dayOfMonth: day++, month: self.calendarMonth(), year: self.calendarYear() }));
                            }
                        }
                    }
                    //self.weeks.push({ days: days });
                    self.weeks.push(calendarWeek);
                }
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