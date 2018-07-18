#if UNITY_IPHONE

namespace UnityEngine.Advertisements {
  using UnityEngine;
  using System.Collections;
  using System.Collections.Generic;

  internal class UnityAdsIos : UnityAdsPlatform {
	public override void init (string gameId, bool testModeEnabled, string gameObjectName) {
		if(Advertisement.UnityDeveloperInternalTestMode) {
			UnityAdsIosBridge.enableUnityDeveloperInternalTestMode();
		}

		UnityAdsIosBridge.init(gameId, testModeEnabled, (Advertisement.debugLevel & Advertisement.DebugLevel.DEBUG) != Advertisement.DebugLevel.NONE ? true : false, gameObjectName);
	}
		
	public override bool show (string zoneId, string rewardItemKey, string options) {
		return UnityAdsIosBridge.show(zoneId, rewardItemKey, options);
	}
		
	public override void hide () {
		UnityAdsIosBridge.hide();
	}
		
	public override bool isSupported () {
		return UnityAdsIosBridge.isSupported();
	}
		
	public override string getSDKVersion () {
		return UnityAdsIosBridge.getSDKVersion();
	}
		
	public override bool canShowZone (string zone) {
		if(!string.IsNullOrEmpty(zone)) {
			return UnityAdsIosBridge.canShowZone(zone);
		} else {
			return UnityAdsIosBridge.canShow();
		}
	}
		
	public override bool hasMultipleRewardItems () {
		return UnityAdsIosBridge.hasMultipleRewardItems();
	}
		
	public override string getRewardItemKeys () {
		return UnityAdsIosBridge.getRewardItemKeys();
	}
		
	public override string getDefaultRewardItemKey () {
		return UnityAdsIosBridge.getDefaultRewardItemKey();
	}
		
	public override string getCurrentRewardItemKey () {
		return UnityAdsIosBridge.getCurrentRewardItemKey();
	}
		
	public override bool setRewardItemKey (string rewardItemKey) {
		return UnityAdsIosBridge.setRewardItemKey(rewardItemKey);
	}
		
	public override void setDefaultRewardItemAsRewardItem () {
		UnityAdsIosBridge.setDefaultRewardItemAsRewardItem();
	}
		
	public override string getRewardItemDetailsWithKey (string rewardItemKey) {
		return UnityAdsIosBridge.getRewardItemDetailsWithKey(rewardItemKey);
	}
		
	public override string getRewardItemDetailsKeys () {
		return UnityAdsIosBridge.getRewardItemDetailsKeys();
	}

	public override void setLogLevel(Advertisement.DebugLevel logLevel) {
		UnityAdsIosBridge.setDebugMode((Advertisement.debugLevel & Advertisement.DebugLevel.DEBUG) != Advertisement.DebugLevel.NONE ? true : false);
	}
  }
}

#endif
