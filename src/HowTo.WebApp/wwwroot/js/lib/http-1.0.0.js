define(["jquery"],
    function ($) {
        "use strict";

        return {
            postFormAsync: function (path, formData) {
                return $.ajax({
                    url: path,
                    method: "POST",
                    data: formData,
                    processData: false,
                    contentType: false,
                }).then(function (data) {
                    if (data.success === true) {
                        return data;
                    } else {
                        throw data.errorMessage;
                    }
                }).catch(function (jqXHR, textStatus, errorThrown) {
                    console.error({ jqXHR: jqXHR, textStatus: textStatus, errorThrown: errorThrown });
                    throw jqXHR.responseText ?? jqXHR;
                });
            },

            postRawAsync: function (path, data) {
                return $.ajax({
                    method: "POST",
                    url: path,
                    data: data,
                    dataType: "json",
                    contentType: "application/json",
                    beforeSend: function (xhr) {
                        xhr.setRequestHeader("Access-Control-Allow-Origin", "*");
                        xhr.setRequestHeader("Authorization", `Bearer ${window.sessionStorage.getItem("accessToken")}`);
                    }
                }).then(function (response) {
                    if (response.success === true) {
                        return response.data;
                    } else {
                        throw response.errorMessage;
                    }
                }).catch(function (jqXHR, textStatus, errorThrown) {
                    console.error({ jqXHR: jqXHR, textStatus: textStatus, errorThrown: errorThrown });
                    throw jqXHR.responseText ?? jqXHR;
                });
            },

            postAsync: function (path, data) {
                return $.ajax({
                        method: "POST",
                        url: path,
                        data: JSON.stringify(data),
                        dataType: "json",
                        contentType: "application/json",
                        beforeSend: function (xhr) {
                            xhr.setRequestHeader("Access-Control-Allow-Origin", "*");
                            xhr.setRequestHeader("Authorization", `Bearer ${window.sessionStorage.getItem("accessToken")}`);
                        }
                    }).then(function (response) {
                        if (response.success === true) {
                            return response.data;
                        } else {
                            throw response.errorMessage;
                        }
                    }).catch(function (jqXHR, textStatus, errorThrown) {
                        console.error({ jqXHR: jqXHR, textStatus: textStatus, errorThrown: errorThrown });
                        throw jqXHR.responseText ?? jqXHR;
                    });
            },

            getAsync: function (path) {
                return $.ajax({
                        url: path,
                        method: "GET",
                        beforeSend: function(xhr){
                            xhr.setRequestHeader("Access-Control-Allow-Origin", "*");
                            xhr.setRequestHeader("Authorization", `Bearer ${window.sessionStorage.getItem("accessToken")}`);
                        }
                    }).then(function (response) {
                        if (response.success === true) {
                            return response.data;
                        } else {
                            throw response.errorMessage;
                        }
                    }).catch(function (jqXHR, textStatus, errorThrown) {
                        console.error({ jqXHR: jqXHR, textStatus: textStatus, errorThrown: errorThrown });
                        throw jqXHR.responseText ?? jqXHR;
                    });
            }
        };
    });