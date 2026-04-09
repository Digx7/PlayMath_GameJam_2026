/*
* This WebGL template is licensed to the purchaser only.
* Unauthorized copying, redistribution, modification, or use of this template or any of its components � in whole or in part � 
* without a valid license is strictly prohibited and constitutes a violation of international copyright laws and licensing terms.
* This notice must remain intact in this file as a condition of use.
* Copyright � Devon Lively, 2025. All rights reserved. 
*/
window.ConstantsLib = {

    ////////////////////////////////////////////////////
    // Display Setup
    ////////////////////////////////////////////////////
    displayScalingMode: 0,
    UseDynamicAspectRatio: false,
    aspectX: 16,          // user wants to set aspect ratio
    aspectY: 9,          // user wants to set aspect ratio
    defaultWidth: 1920,
    defaultHeight: 1080,
    PageSetup_MaxDPI: 2,
    PageSetup_DefaultDPR: 1,
    fullscreenHidesCornerButton: true,

    ////////////////////////////////////////////////////
    // Loading Screen
    ////////////////////////////////////////////////////
    ShowLoadingLogoImage: true,					 // show loading logo image
    loadingScreenColor: "#191919",          // webpage background
    progressBarFullSettings: "#1EAED3",            // loading bar color
    LoadingScreen_BarPulseColor: '#1E3ED3',
    LoadingScreen_PercentValueTextColorLow: '#BFBFBF',
    LoadingScreen_PercentValueTextColorHigh: '#191919',
    loadingBarPulses: true,

    ////////////////////////////////////////////////////
    // Shared Tap + Loading Settings
    ////////////////////////////////////////////////////
    showBackgroundImage: true,

    ////////////////////////////////////////////////////
    // Hamburger / Corner Button
    ////////////////////////////////////////////////////
    hideCornerButton: false,                     // hide fullscreen/menu button

    ////////////////////////////////////////////////////
    // Page Setup
    ////////////////////////////////////////////////////
    disableSaveMenuPopup: true,
    disableCtrlWClosesTab: [[[PageSetup_DisableCtrlWClosesTab]]],
    pageTitle: "Name Seen In Browser Tab",

    ////////////////////////////////////////////////////
    // Google Fonts
    ////////////////////////////////////////////////////
    googleFontHref: "https://fonts.googleapis.com/css2?family=Roboto:ital,wght@0,100..900;1,100..900&display=swap",                      // link to font
    googleFontFamily: '"Roboto", sans-serif',                  // font family
}
