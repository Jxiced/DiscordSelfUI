window.onload = function () {
    const messageElement = document.getElementById("message");
    const maxMessageElement = document.getElementById("maxLength");
    const errorAlertContainerElement = document.getElementById("messageLengthAlert");

    if (messageElement && maxMessageElement && errorAlertContainerElement) {
        const message = messageElement as HTMLInputElement;
        const maxLength = Number((maxMessageElement as HTMLInputElement).value);
        const errorAlert = errorAlertContainerElement as HTMLDivElement;

        message.addEventListener("input", () => {
            errorAlert.style.display = message.value.length > maxLength ? "block" : "none";
        });
    }
};

const clearButtonElement = document.getElementById("clearButton");

if (clearButtonElement) {
    const clearButton = clearButtonElement as HTMLButtonElement;

    clearButton.onclick = function () {
        const messageElement = document.getElementById("message");

        if (messageElement) {
            const message = messageElement as HTMLInputElement;
            message.value = "";
        }
    }
}

const messageFormElement = document.getElementById("messageForm");

if (messageFormElement) {
    const messageForm = messageFormElement as HTMLFormElement;

    messageForm.addEventListener("submit", (e: Event) => {
        e.preventDefault();

        const messageElement = document.getElementById("message");
        const ttsElement = document.getElementById("ttsCheckbox");
        const modelIdElement = document.getElementById("modelId");
        const errorAlertContainerElement = document.getElementById("errorAlertContainer");

        if (messageElement && ttsElement && modelIdElement && errorAlertContainerElement) {
            const message = messageElement as HTMLInputElement;
            const tts = ttsElement as HTMLInputElement;
            const modelId = modelIdElement as HTMLInputElement;
            const errorAlertContainer = errorAlertContainerElement as HTMLDivElement;

            const modelIdValue = modelId.value;

            let formData = new FormData();
            formData.append("Message", message.value);
            formData.append("TTS", tts.checked.toString());

            fetch(`/User/${modelIdValue}/Message`, {
                method: "POST",
                body: formData
            }).then(resp => {
                if (resp.redirected) {
                    return resp.text();
                }

                return resp.json();
            }).then((data) => {
                if (typeof data === "string") {
                    errorAlertContainer.style.display = "none";

                    message.value = "";

                    message.dispatchEvent(new Event("change"));
                } else {
                    if (data.error) {
                        errorAlertContainer.style.display = "block";
                    } 
                }
            });
        }
    });
}
