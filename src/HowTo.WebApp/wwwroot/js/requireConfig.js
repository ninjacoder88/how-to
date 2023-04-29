let appVersion = document.getElementById("app-version");

requirejs.config({
    urlArgs: appVersion.value,
    baseUrl: "/js",
    paths: {
        bootstrap: "lib/bootstrap.bundle-5.1.3.min",
        http: "lib/http-1.0.0",
        jquery: "lib/jquery-3.6.3.min",
        knockout: "lib/knockout-3.5.0.min",
        pubsub: "lib/pubsub-1.0.0",
        text: "lib/text-2.0.16",
        howToModal: "lib/how-to-modal/howToModal"
    }
});