window.onload = function () {
    var messageElement = document.getElementById("message");
    var maxMessageElement = document.getElementById("maxLength");
    var errorAlertContainerElement = document.getElementById("messageLengthAlert");
    if (messageElement && maxMessageElement && errorAlertContainerElement) {
        var message_1 = messageElement;
        var maxLength_1 = Number(maxMessageElement.value);
        var errorAlert_1 = errorAlertContainerElement;
        message_1.addEventListener("input", function () {
            errorAlert_1.style.display = message_1.value.length > maxLength_1 ? "block" : "none";
        });
    }
};
var clearButtonElement = document.getElementById("clearButton");
if (clearButtonElement) {
    var clearButton = clearButtonElement;
    clearButton.onclick = function () {
        var messageElement = document.getElementById("message");
        if (messageElement) {
            var message = messageElement;
            message.value = "";
        }
    };
}
var messageFormElement = document.getElementById("messageForm");
if (messageFormElement) {
    var messageForm = messageFormElement;
    messageForm.addEventListener("submit", function (e) {
        e.preventDefault();
        var messageElement = document.getElementById("message");
        var ttsElement = document.getElementById("ttsCheckbox");
        var modelIdElement = document.getElementById("modelId");
        var errorAlertContainerElement = document.getElementById("errorAlertContainer");
        if (messageElement && ttsElement && modelIdElement && errorAlertContainerElement) {
            var message_2 = messageElement;
            var tts = ttsElement;
            var modelId = modelIdElement;
            var errorAlertContainer_1 = errorAlertContainerElement;
            var modelIdValue = modelId.value;
            var formData = new FormData();
            formData.append("Message", message_2.value);
            formData.append("TTS", tts.checked.toString());
            fetch("/User/".concat(modelIdValue, "/Message"), {
                method: "POST",
                body: formData
            }).then(function (resp) {
                if (resp.redirected) {
                    return resp.text();
                }
                return resp.json();
            }).then(function (data) {
                if (typeof data === "string") {
                    errorAlertContainer_1.style.display = "none";
                    message_2.value = "";
                }
                else {
                    if (data.error) {
                        errorAlertContainer_1.style.display = "block";
                    }
                }
            });
        }
    });
}
//# sourceMappingURL=messageUser.js.map