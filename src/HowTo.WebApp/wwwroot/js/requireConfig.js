let appVersion = document.getElementById("app-version");

requirejs.config({
    urlArgs: appVersion.value,
    baseUrl: "/js/lib",
    paths: {
        bootstrap: "bootstrap.bundle-5.1.3.min",
        httpService: "httpService-1.0.0",
        jquery: "jquery-3.6.3.min",
        knockout: "knockout-3.5.0.min",
        pubsub: "pubsub-1.0.0",
        text: "text-2.0.16",
        howToModal: "how-to-modal/howToModal"
    }
});