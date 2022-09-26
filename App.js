/**
 * Sample React Native App
 * https://github.com/facebook/react-native
 *
 * @format
 * @flow strict-local
 */

 import React, {useRef} from 'react';
 import type {Node} from 'react';
 import {
   SafeAreaView,   
   ScrollView,
   StatusBar,
   StyleSheet,
   Text,
   useColorScheme,
   View,
   NativeModules,
   TouchableOpacity,
   Clipboard,
   Linking,
 } from 'react-native';
 
 import {   
   Colors,
   DebugInstructions,
   Header,
   LearnMoreLinks,
   ReloadInstructions,
 } from 'react-native/Libraries/NewAppScreen';
 
 import { WebView } from "react-native-webview";
 
 const App: () => Node = () => {
   // const webviewRef = useRef();
   const isDarkMode = useColorScheme() === 'dark';
 
   const _onPressHandler = async () => {
     // Calling FancyMath.add method
     await NativeModules.FancyMath.add(1,2);
     console.error('hi i was called')
   }
   const _onMessage = (event) => {

     // const data = JSON.parse(event.nativeEvent.data);
     if (event.nativeEvent.data.startsWith('Paste')) {
       Clipboard.setString(event.nativeEvent.data);
       NativeModules.FancyMath.add(1,2);
     }
     else if (event.nativeEvent.data.startsWith('createNewNoteFromPrev')) {
      executeScript();
   }
   else if (event.nativeEvent.data.startsWith('openUpsellExtension')) {
    Linking.openURL('https://chrome.google.com/webstore/detail/onenote-web-clipper/gojbdfnpnhogfdgjbigejoaolejmgdhk?hl=en')
   }
   }
 
   const sendDataToWebView = () => {
     webviewRef.current.postMessage('Data from React Native App');
     window.alert('fffff');
 
   }

   const addPreview = () => {
    webviewRef.current.injectJavaScript(`
     var link = "http://www.quirksmode.org/iframetest2.html"
     var iframe = document.createElement('iframe');
iframe.frameBorder=0;
iframe.height="250px";
iframe.width="480px";
iframe.id="randomid";
iframe.setAttribute("srcdoc", '<style>body {font-family: "Segoe UI";} img {  max-height: "200px"; display: "block";max-width: 30%;} </style><html><img src="https://upload.wikimedia.org/wikipedia/commons/9/90/Hapus_Mango.jpg" /><p>Medium is an open platform where readers find dynamic thinking, and where expert and undiscovered voices can share their writing on any topic.</p><p>Some other gibberish stuff.</p></html>');
document.getElementById("add54").prepend(iframe);
document.getElementById("noteTitle").innerText = "Medium.com";`)
   }
 
   const executeScript = async () => {
    //  let data = await Clipboard.getString();
    let contextData1 = '{"title": "Wikipedia of Mango","iconSrc": "https://upload.wikimedia.org/wikipedia/commons/9/90/Hapus_Mango.jpg","pageUrl": "https://www.wikipedia.com","firstImgSrc": "https://upload.wikimedia.org/wikipedia/commons/9/90/Hapus_Mango.jpg"}'
    let contextData = '{"title": "Wikipedia of Mango","iconSrc": "https://upload.wikimedia.org/wikipedia/commons/9/90/Hapus_Mango.jpg","pageUrl": "https://www.wikipedia.com","firstImgSrc": "https://upload.wikimedia.org/wikipedia/commons/9/90/Hapus_Mango.jpg","description": "Botanically, mango is a drupe, consisting of an outer skin, a fleshy edible portion, and a central stone enclosing a single seed also called stone fruit, like a plum, cherry, or peach."}'
     let contextDataParsed = JSON.parse(contextData)
     const data = `<html><img src=${contextDataParsed.firstImgSrc} /><p>${contextDataParsed.description}</p><p>Some other gibberish stuff.</p></html>`
     webviewRef.current &&
       webviewRef.current.injectJavaScript(
         `window.CreateNewNoteExtend("Gray",${JSON.stringify(data)}, ${contextData1});`
       );
   }

  //  const runServer = () => {
  //   NativeModules.FancyMath.runServer();
  //   window.alert('aaa');
  //  }
 
   const webviewRef = useRef();
 
   return (
     <SafeAreaView style={styles.containerStyle}>
       {/* <Text style={{fontSize: 15, color: 'white', marginBottom: 10, marginTop: 10}}>
             Here's something we captured, save it?
           </Text> */}
       {/* <WebView
       ref={webViewRefPreview}
       style={showIt ? {flex: 1, maxHeight: 300, marginLeft: 25, marginRight: 25, borderRadius: 20, borderWidth: 1} : {display: 'none'}}
       useWebKit={true}
         // keyboardDisplayRequiresUserAction={true}
         originWhitelist={["*"]}
         useWebView2={true}
         source={{html: '<style>body {font-family: "Segoe UI";} img { display: block; max-width: 100%; max-height: 200; } </style><html><img src="https://upload.wikimedia.org/wikipedia/commons/9/90/Hapus_Mango.jpg" /><p>Medium is an open platform where readers find dynamic thinking, and where expert and undiscovered voices can share their writing on any topic.</p><p>Some other gibberish stuff.</p></html>'}} 
       /> */}
       {/* <TouchableOpacity
           onPress={runServer}
           style={{
             alignItems: 'center'
           }}>
           <Text style={{fontSize: 20, color: 'white', backgroundColor: '#7719aa'}}>
             Save Note
           </Text>
         </TouchableOpacity> */}
       <WebView
       ref={webviewRef}
       style={{flex: 1}}
         source={{
           uri: "https://www.onenote.com/stickynotesstaging?localDevOverride=https%3A%2F%2F3ba3-2404-f801-8028-3-ad2f-b9f5-85e9-a2df.ngrok.io%2F",
         }}
         onMessage={_onMessage}
         useWebKit={true}
         // keyboardDisplayRequiresUserAction={true}
         originWhitelist={["*"]}
         useWebView2={true}
         onLoad={()=>{setTimeout(function(){
          addPreview();
        }, 1000);}}
       />
     </SafeAreaView>
   );
 };
 
 const styles = StyleSheet.create({
   containerStyle: {
     flex: 1,
   },
 });
 export default App;