define(["bootstrap", "text!octavius-modal/octavius-modal.html"],
    function (bs, html) {
        "use strict";

        var modalsElement = document.getElementById("modals");
        modalsElement.innerHTML = html;

        return {
            self: this,

            showModal: function (obj) {
                var titleElement = document.getElementById("how-to-modal-title");
                var messageElement = document.getElementById("how-to-modal-message");

                if (obj.title) {
                    titleElement.innerText = obj.title;
                }
                if (obj.message) {
                    messageElement.innerText = obj.message;
                }

                let howToModal = new bs.Modal(document.getElementById("how-to-modal"));
                howToModal.show();
            }
        };
    });