const container = document.querySelector("#unity-container");
const canvas = document.querySelector("#unity-canvas");
const loadingOverlay = document.querySelector("#loading-overlay");
const progressBarFill = document.querySelector("#unity-progress-bar-fill");
const progressValue = document.querySelector("#unity-progress-value");
const progressText = document.querySelector("#unity-progress-text");
const loadingTip = document.querySelector("#loading-tip");
const mascotAnimation = document.querySelector("#mascot-animation");
const initialSpinner = document.querySelector("#initial-spinner");
const rainbowProgressFill = document.querySelector("#rainbow-progress-fill");

// Make sure the spinner is visible immediately
if (initialSpinner) {
    initialSpinner.style.display = "block";
    initialSpinner.style.zIndex = "9999";
}

// Hide canvas initially to prevent black screen
if (canvas) {
    canvas.style.visibility = "hidden";
}

const loadingTips = [
  "Access thousands of games instantly through Wortal on web, mobile, and super-apps!",
  "Play anywhere, anytime—Wortal games work seamlessly across devices without downloads!",
  "Wortal offers a diverse collection of games—action, puzzles, racing, sports, and more!",
  "Discover new games constantly—Wortal.Games regularly updates its collection!",
  "Wortal.Games is family-friendly and designed with built-in safety features!",
  "No app hopping needed—find all your favorite games in one unified hub, Wortal!",
  "Join a global community of over 4 billion players across the Wortal platform!",
  "Wortal uses AI-powered technology to enhance your gaming experience!",
  "Wortal partners with k-ID for comprehensive age verification and privacy protection!",
  "Switch between platforms seamlessly—Wortal works on virtually any modern device!"
];

// Initialize loading screen right away
window.addEventListener("DOMContentLoaded", function() {
    // Make sure spinner is visible and loading overlay is hidden until Unity starts loading
    if (initialSpinner) initialSpinner.style.display = "block";
    if (loadingOverlay) loadingOverlay.style.visibility = "hidden";
    
    // Start showing loading tips immediately
    if (loadingTip) loadingTip.textContent = loadingTips[0];
});

window.addEventListener("load", function() {
    if ("serviceWorker" in navigator) {
        navigator.serviceWorker.register("assets/js/service-worker.js");
    }
});

const loaderUrl = "Build/{{{ LOADER_FILENAME }}}";
const config = {
    dataUrl: "Build/{{{ DATA_FILENAME }}}",
    frameworkUrl: "Build/{{{ FRAMEWORK_FILENAME }}}",
#if USE_THREADS
    workerUrl: buildUrl + "/{{{ WORKER_FILENAME }}}",
#endif
#if USE_WASM
    codeUrl: "Build/{{{ CODE_FILENAME }}}",
#endif
#if MEMORY_FILENAME
    memoryUrl: "Build/{{{ MEMORY_FILENAME }}}",
#endif
#if SYMBOLS_FILENAME
    symbolsUrl: "Build/{{{ SYMBOLS_FILENAME }}}",
#endif
    streamingAssetsUrl: "StreamingAssets",
    companyName: {{{ JSON.stringify(COMPANY_NAME) }}},
    productName: {{{ JSON.stringify(PRODUCT_NAME) }}},
    productVersion: {{{ JSON.stringify(PRODUCT_VERSION) }}},
    // matchWebGLToCanvasSize: false, // Uncomment this to separately control WebGL canvas render size and DOM element size.
    // devicePixelRatio: 1, // Uncomment this to override low DPI rendering on high DPI displays.
};

// Mobile device style - fill the whole browser client area with the game canvas.
if (/iPhone|iPad|iPod|Android/i.test(navigator.userAgent)) {
    const meta = document.createElement("meta");
    meta.name = "viewport";
    meta.content = "width=device-width, height=device-height, initial-scale=1.0, user-scalable=no, shrink-to-fit=yes";
    document.getElementsByTagName("head")[0].appendChild(meta);
}

#if BACKGROUND_FILENAME
canvas.style.background = "url('" + buildUrl + "/{{{ BACKGROUND_FILENAME.replace(/'/g, '%27') }}}') center / cover";
#endif

//////////////////////////////////////////
// Enhanced Loading Animation
//////////////////////////////////////////

// Update loading tip periodically
let tipIndex = 0;
setInterval(() => {
    if (loadingTip) {
        tipIndex = (tipIndex + 1) % loadingTips.length;
        // Animate tip change with fade effect
        loadingTip.style.opacity = 0;
        setTimeout(() => {
            loadingTip.textContent = loadingTips[tipIndex];
            loadingTip.style.opacity = 1;
        }, 300);
    }
}, 5000); // Show tips more frequently (5 seconds)

// Animate dots for loading text
let dotCount = 0;
let dots = "";

const dotInterval = setInterval(() => {
    if (dotCount > 3) {
        dotCount = 0;
    }

    dotCount++;
    dots = new Array(dotCount % 4 + 1).join('.');

    // Only update the dots part of the text to avoid overwriting our stage descriptions
    if (progressText && progressText.textContent && progressText.textContent.includes("...")) {
        progressText.textContent = progressText.textContent.replace(/\.+$/, dots);
    } else if (progressText) {
        progressText.textContent = progressText.textContent + dots;
    }
}, 500);

const progressTexts = {
    default: "Loading game",
    preparing: "Preparing environment",
    loading: "Loading game assets",
    initializing: "Initializing Wortal SDK",
    optimizing: "Optimizing game experience",
    finishing: "Almost there"
};

function updateProgressText(value) {
    let baseText;

    if (value < 20) {
        baseText = progressTexts.preparing;
    } else if (value < 40) {
        baseText = progressTexts.loading;
    } else if (value < 60) {
        baseText = progressTexts.initializing;
    } else if (value < 85) {
        baseText = progressTexts.optimizing;
    } else {
        baseText = progressTexts.finishing;
    }

    if (progressText) progressText.innerText = baseText + dots;
}

// Update rainbow progress visualization for umbrella effect
function updateRainbowProgress(progress) {
    const value = progress * 100; 

    // Update rainbow arc visibility
    updateRainbowArc(value);
    
    // For umbrella effect - reveal from top to bottom
    if (rainbowProgressFill) {
        const revealedPercentage = 100 - value;
        const clipPath = `polygon(0% 0%, 0% ${revealedPercentage}%, 100% ${revealedPercentage}%, 100% 0%)`;
        rainbowProgressFill.style.clipPath = clipPath;
    }

    // Animate the rainbow segments
    const segments = document.querySelectorAll('.rainbow-segment');
    segments.forEach((segment, index) => {
        // Calculate delay for each segment
        const segmentProgress = Math.max(0, Math.min(1, (value - (index * 10)) / 20));
        segment.style.opacity = 0.3 + (segmentProgress * 0.7);
        
        // Staggered animation effect
        if (value > index * 12) {
            segment.style.strokeDasharray = '283';
            segment.style.strokeDashoffset = 283 - (283 * segmentProgress);
        }
    });

    // Add movement animation to mascot during mid-loading to keep users engaged
    if (value >= 40 && value <= 85 && mascotAnimation) {
        if (!mascotAnimation.style.animation || mascotAnimation.style.animation === 'none') {
            // Apply gentle floating animation during this period
            mascotAnimation.style.animation = 'mascot-float 3s ease-in-out infinite';
        }
    }

    // Add a slight bounce effect when reaching 100%
    if (value >= 99.5 && mascotAnimation) {
        mascotAnimation.style.animation = 'mascot-bounce 1s ease 3';
        mascotAnimation.classList.add('bounced');

        // Add a celebration effect when loading completes
        const segments = document.querySelectorAll('.rainbow-segment');
        if (segments.length) {
            segments.forEach(segment => {
                segment.style.filter = "saturate(2) brightness(1.2)";
            });
            setTimeout(() => {
                segments.forEach(segment => {
                    segment.style.filter = "saturate(1.5)";
                });
            }, 3000);
        }
    }
}

function updateRainbowArc(percent) {
    const arcs = document.querySelectorAll('.rainbow-segment');
    if (!arcs.length) return;

    const totalLength = 283; // Matches SVG path length
    
    arcs.forEach((arc, index) => {
        // Stagger the progress so each color appears with a delay
        const arcPercent = Math.max(0, percent - (index * 5));
        const progressLength = (arcPercent / 100) * totalLength;
        arc.style.strokeDasharray = totalLength;
        arc.style.strokeDashoffset = totalLength - progressLength;
    });
}

//////////////////////////////////////////
// Enhanced Progressive Loading with Staged Progress
//////////////////////////////////////////

// Stage definitions with timing and progress ranges
const loadingStages = [
    { name: "initialization", duration: 3000, start: 0, end: 30 },
    { name: "assets", duration: 4000, start: 30, end: 60 },
    { name: "optimization", duration: 5000, start: 60, end: 75 },
    { name: "finalization", duration: 6000, start: 75, end: 90 } // Cap at 90% until actual completion
];

// Simulated loading progress variables
let simulatedProgress = 0;
let actualProgress = 0;
let currentStage = 0;
let stageStartTime = 0;
let isSimulating = true;

// Start simulated loading progress
function startSimulatedProgress() {
    stageStartTime = Date.now();
    isSimulating = true;
    simulateNextStage();
}

// Simulate loading progress through predefined stages
function simulateNextStage() {
    if (!isSimulating || currentStage >= loadingStages.length) return;
    
    const stage = loadingStages[currentStage];
    const elapsed = Date.now() - stageStartTime;
    const stageDuration = stage.duration;
    const stageRange = stage.end - stage.start;
    
    // Calculate progress within this stage (0-1)
    let stageProgress = Math.min(1, elapsed / stageDuration);
    
    // Apply easing function for more natural progress (slow start, faster middle, slow end)
    stageProgress = easeInOutCubic(stageProgress);
    
    // Calculate overall progress value (0-100)
    simulatedProgress = stage.start + (stageProgress * stageRange);
    
    // Update UI
    updateLoadingUI(simulatedProgress / 100);
    
    // Move to next stage or continue current
    if (elapsed >= stageDuration) {
        currentStage++;
        stageStartTime = Date.now();
    }
    
    // Continue until real loading takes over or we're done with stages
    if (currentStage < loadingStages.length && isSimulating) {
        requestAnimationFrame(simulateNextStage);
    }
}

// Easing function for smoother progress animation
function easeInOutCubic(t) {
    return t < 0.5 ? 4 * t * t * t : 1 - Math.pow(-2 * t + 2, 3) / 2;
}

// Update loading UI based on progress value (0-1)
function updateLoadingUI(progress) {
    const value = Math.round(progress * 100);
    if (progressBarFill) progressBarFill.style.width = value + '%';
    if (progressValue) progressValue.innerText = value + '%';
    updateProgressText(value);
    updateRainbowProgress(progress);
    
    // Add subtle animation to mascot at certain progress points
    if (mascotAnimation) {
        if (value % 20 === 0 && !mascotAnimation.classList.contains('pulsed-' + value)) {
            mascotAnimation.classList.add('pulsed-' + value);
            mascotAnimation.style.transform = 'translateX(-50%) scale(1.1)';
            setTimeout(() => {
                mascotAnimation.style.transform = 'translateX(-50%) scale(1)';
            }, 300);
        }
    }
}

//////////////////////////////////////////
// Unity loader with Wortal SDK Integration
//////////////////////////////////////////

let gameInstance;
window.isUnitySDKInitialized = false;

console.log("Setting up Unity loading sequence");

const script = document.createElement("script");
script.src = loaderUrl;
script.onload = () => {
    console.log("Unity loader script loaded");
    
    // Make loading overlay visible now
    if (loadingOverlay) loadingOverlay.style.visibility = "visible";
    
    // Start simulated loading progress
    startSimulatedProgress();
    
    // Starting Wortal initialization
    console.log("Initializing Wortal SDK");
    window.Wortal.initializeAsync().then(() => {
        console.log("Wortal SDK initialized successfully");
        
        // Make canvas visible now that we're starting Unity
        if (canvas) canvas.style.visibility = "visible";
        
        // Hide the initial spinner when the proper loading screen is showing
        if (initialSpinner) initialSpinner.style.display = "none";

        createUnityInstance(canvas, config, (progress) => {
            // Store actual Unity loading progress
            actualProgress = progress * 100;
            window.Wortal.setLoadingProgress(actualProgress);
            
            // Only switch to actual progress when it exceeds our simulated progress
            // if (actualProgress > simulatedProgress && actualProgress > 89) {
            //    isSimulating = false;
            //    updateLoadingUI(progress);
            // }
            
            // Ensure we never appear fully loaded (90% max) until actually done
            if (progress >= 0.94 && progress < 1) {
                // Gently pulse the mascot while waiting at ~90%
                if (mascotAnimation && !mascotAnimation.classList.contains('waiting')) {
                    mascotAnimation.classList.add('waiting');
                    mascotAnimation.style.animation = 'mascot-pulse 2s ease-in-out infinite';
                }
            }

        }).then((unityInstance) => {
            clearInterval(dotInterval);
            
            // Ensure we show 100% at completion
            simulatedProgress = 100;
            updateLoadingUI(1.0);
            window.Wortal.setLoadingProgress(100);

            gameInstance = unityInstance;
            window.isUnitySDKInitialized = true;

            window.Wortal.startGameAsync().catch(error => {
                console.error("Error starting game:", error);
            });

            // Add a small delay before hiding the loading screen
            setTimeout(() => {
                // Fade out loading overlay
                if (loadingOverlay) {
                    loadingOverlay.style.transition = "opacity 0.8s ease";
                    loadingOverlay.style.opacity = "0";

                    // Remove from DOM after fade out
                    setTimeout(() => {
                        loadingOverlay.style.display = "none";
                    }, 800);
                }
            }, 500);

        }).catch(error => {
            console.error("Error creating Unity instance:", error);
            // Show error message to user
            if (progressText) {
                progressText.innerText = "Error loading game";
                progressText.style.color = "red";
            }
        });
    }).catch(error => {
        console.error("Error initializing Wortal:", error);
        // Show Wortal SDK error message
        if (progressText) {
            progressText.innerText = "Error initializing SDK";
            progressText.style.color = "red";
        }
    });
};

document.body.appendChild(script);