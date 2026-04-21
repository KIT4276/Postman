mergeInto(LibraryManager.library, {
    YG_Init: function (goNamePtr) {
        try {
            var goName = UTF8ToString(goNamePtr);

            window.__ygBridge = window.__ygBridge || {
                goName: null,
                ysdk: null,
                player: null,
                initialized: false,
                pauseResumeSubscribed: false,
                send: null
            };

            var bridge = window.__ygBridge;
            bridge.goName = goName;

            function send(method, value) {
                try {
                    var payload = (value === undefined || value === null) ? "" : String(value);

                    if (typeof SendMessage === "function") {
                        SendMessage(bridge.goName, method, payload);
                        return;
                    }

                    if (typeof unityInstance !== "undefined" && unityInstance && unityInstance.SendMessage) {
                        unityInstance.SendMessage(bridge.goName, method, payload);
                        return;
                    }

                    if (typeof window !== "undefined" &&
                        window.unityInstance &&
                        window.unityInstance.SendMessage) {
                        window.unityInstance.SendMessage(bridge.goName, method, payload);
                    }
                } catch (e) {
                    console.error("[YG->Unity] SendMessage error:", method, e);
                }
            }

            bridge.send = send;

            function installPauseResumeHandlers() {
                if (!bridge.ysdk || !bridge.ysdk.on || bridge.pauseResumeSubscribed) {
                    return;
                }

                bridge.pauseCallback = function () {
                    bridge.send("OnGameApiPause", "");
                };

                bridge.resumeCallback = function () {
                    bridge.send("OnGameApiResume", "");
                };

                bridge.ysdk.on("game_api_pause", bridge.pauseCallback);
                bridge.ysdk.on("game_api_resume", bridge.resumeCallback);
                bridge.pauseResumeSubscribed = true;
            }

            if (bridge.initialized) {
                var cachedLang = "en";
                installPauseResumeHandlers();
                send("OnYsdkInitOk", "");

                if (bridge.ysdk &&
                    bridge.ysdk.environment &&
                    bridge.ysdk.environment.i18n &&
                    bridge.ysdk.environment.i18n.lang) {
                    cachedLang = bridge.ysdk.environment.i18n.lang;
                }

                send("OnLanguageDetected", cachedLang);
                send("OnPlayerReady", bridge.player ? "1" : "0");
                return;
            }

            if (typeof YaGames === "undefined" || !YaGames.init) {
                send("OnYsdkInitError", "YaGames SDK not found");
                return;
            }

            YaGames.init()
                .then(function (ysdk) {
                    var lang = "en";

                    bridge.ysdk = ysdk;
                    bridge.initialized = true;
                    installPauseResumeHandlers();

                    send("OnYsdkInitOk", "");

                    if (ysdk &&
                        ysdk.environment &&
                        ysdk.environment.i18n &&
                        ysdk.environment.i18n.lang) {
                        lang = ysdk.environment.i18n.lang;
                    }

                    send("OnLanguageDetected", lang);

                    if (ysdk.getPlayer) {
                        return ysdk.getPlayer({ scopes: false });
                    }

                    return null;
                })
                .then(function (player) {
                    bridge.player = player || null;
                    send("OnPlayerReady", player ? "1" : "0");
                })
                .catch(function (e) {
                    send("OnYsdkInitError", e && e.message ? e.message : String(e));
                });
        } catch (e) {
            console.error("[YG_Init] fatal error:", e);
        }
    },

    YG_Ready: function () {
        try {
            var bridge = window.__ygBridge;
            if (!bridge || !bridge.ysdk) {
                return;
            }

            if (bridge.ysdk.features &&
                bridge.ysdk.features.LoadingAPI &&
                bridge.ysdk.features.LoadingAPI.ready) {
                bridge.ysdk.features.LoadingAPI.ready();
            }
        } catch (e) {
            console.error("[YG_Ready] error:", e);
        }
    },

    YG_ShowFullscreenAdv: function () {
        try {
            var bridge = window.__ygBridge;
            if (!bridge || !bridge.ysdk || !bridge.ysdk.adv) {
                if (bridge && bridge.send) {
                    bridge.send("OnAdError", "Fullscreen adv unavailable");
                }
                return;
            }

            bridge.ysdk.adv.showFullscreenAdv({
                callbacks: {
                    onOpen: function () {
                        bridge.send("OnAdOpen", "");
                    },
                    onClose: function (wasShown) {
                        bridge.send("OnAdClose", wasShown ? "1" : "0");
                    },
                    onError: function (e) {
                        bridge.send("OnAdError", e && e.message ? e.message : String(e));
                    },
                    onOffline: function () {
                        bridge.send("OnAdError", "Offline");
                    }
                }
            });
        } catch (e) {
            console.error("[YG_ShowFullscreenAdv] fatal error:", e);
            var bridge = window.__ygBridge;
            if (bridge && bridge.send) {
                bridge.send("OnAdError", e && e.message ? e.message : String(e));
            }
        }
    },

    YG_ShowRewardedVideo: function () {
        try {
            var bridge = window.__ygBridge;
            if (!bridge || !bridge.ysdk || !bridge.ysdk.adv) {
                if (bridge && bridge.send) {
                    bridge.send("OnRvError", "Rewarded adv unavailable");
                }
                return;
            }

            bridge.ysdk.adv.showRewardedVideo({
                callbacks: {
                    onOpen: function () {
                        bridge.send("OnRvOpen", "");
                    },
                    onRewarded: function () {
                        bridge.send("OnRvReward", "");
                    },
                    onClose: function () {
                        bridge.send("OnRvClose", "");
                    },
                    onError: function (e) {
                        bridge.send("OnRvError", e && e.message ? e.message : String(e));
                    }
                }
            });
        } catch (e) {
            console.error("[YG_ShowRewardedVideo] fatal error:", e);
            var bridge = window.__ygBridge;
            if (bridge && bridge.send) {
                bridge.send("OnRvError", e && e.message ? e.message : String(e));
            }
        }
    }
});
