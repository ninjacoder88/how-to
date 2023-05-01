define([],
    function () {
        "use strict";

        let queues = {};
        return {
            publish: function (queueName, message) {
                if (queues[queueName] === undefined) {
                    queues[queueName] = [];
                }
                queues[queueName].forEach(q => {
                    q(message);
                });
            },
            subscribe: function (queueName, callback) {
                if (queues[queueName] === undefined) {
                    queues[queueName] = [];
                }
                queues[queueName].push(callback);
            }
        };
    });