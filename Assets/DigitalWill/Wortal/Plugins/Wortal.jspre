Module['Wortal'] = Module['Wortal'] || {};

// Global pointers

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

Module['Wortal'].TriggerNoShow = function() {
  dynCall_v(Module.Wortal.noShowPointer);
}

// AdSense specific pointers

Module['Wortal'].TriggerAdBreakDone = function() {
  dynCall_v(Module.Wortal.adBreakDonePointer);
}

Module['Wortal'].TriggerBeforeReward = function() {
  dynCall_v(Module.Wortal.beforeRewardPointer);
}
