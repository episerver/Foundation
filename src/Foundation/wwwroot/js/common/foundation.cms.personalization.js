import HeroBlockTracking from "Features/Blocks/HeroBlock/heroblock-tracking";
import VideoBlockTracking from "Features/Blocks/VideoBlock/videoblock-tracking";

export default class FoundationCmsPersonalization {
    init() {
        let heroBlockTracking = new HeroBlockTracking();
        heroBlockTracking.init();

        let videoBlockTracking = new VideoBlockTracking();
        videoBlockTracking.init();
    }
}