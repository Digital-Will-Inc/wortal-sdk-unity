Module['Wortal'] = Module['Wortal'] || {};

Module['Wortal'].TriggerBeforeAd = function() {
    dynCall_v(Module.Wortal.beforeAdPointer);
}

Module['Wortal'].TriggerAfterAd = function() {
    dynCall_v(Module.Wortal.afterAdPointer);
}

Module['Wortal'].TriggerAdDismissed = function() {
    dynCall_v(Module.Wortal.adDismissedPointer);
}

Module['Wortal'].TriggerAdViewed = function() {
    dynCall_v(Module.Wortal.adViewedPointer);
}
