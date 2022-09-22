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
     console.error(" message start", event);
     // const data = JSON.parse(event.nativeEvent.data);
     if (event.nativeEvent.data.startsWith('Paste')) {
       Clipboard.setString(event.nativeEvent.data);
       NativeModules.FancyMath.add(1,2);
     }
   }
 
   const sendDataToWebView = () => {
     webviewRef.current.postMessage('Data from React Native App');
     window.alert('fffff');
 
   }
 
   const executeScript = async () => {
     let data = await Clipboard.getString();
     data = '<html><img src="https://upload.wikimedia.org/wikipedia/commons/9/90/Hapus_Mango.jpg" /><p>Medium is an open platform where readers find dynamic thinking, and where expert and undiscovered voices can share their writing on any topic.</p><p>Some other gibberish stuff.</p></html>';
     webviewRef.current &&
       webviewRef.current.injectJavaScript(
         `window.CreateNewNoteExtend("Gray",${JSON.stringify(data)});`
       );
   }
 
   const webviewRef = useRef();
 
   return (
     <SafeAreaView style={styles.containerStyle}>
       <Text style={{fontSize: 15, color: 'white', marginBottom: 10, marginTop: 10}}>
             Here's something we captured, save it?
           </Text>
       <WebView
       style={{flex: 1, maxHeight: 300, marginLeft: 25, marginRight: 25, borderRadius: 20, borderWidth: 1}}
         source={{html: '<style>body {font-family: "Segoe UI";} img { display: block; max-width: 100%; height: 200; } </style><html><img src="https://upload.wikimedia.org/wikipedia/commons/9/90/Hapus_Mango.jpg" /><p>Medium is an open platform where readers find dynamic thinking, and where expert and undiscovered voices can share their writing on any topic.</p><p>Some other gibberish stuff.</p></html>'}} 
       />
       <TouchableOpacity
           onPress={executeScript}
           style={{
             alignItems: 'center'
           }}>
           <Text style={{fontSize: 20, color: 'white', backgroundColor: '#7719aa'}}>
             Save Note
           </Text>
         </TouchableOpacity>
       <WebView
       ref={webviewRef}
       style={{flex: 1}}
         source={{
           uri: "https://www.onenote.com/stickynotesstaging?localDevOverride=https%3A%2F%2F1bc4-2404-f801-8028-1-3c20-9932-a3d1-656f.ngrok.io%2F",
         }}
         onMessage={_onMessage}
         useWebKit={true}
         // keyboardDisplayRequiresUserAction={true}
         originWhitelist={["*"]}
         useWebView2={true}
         onLoad={()=>{console.error('WebViewLoaded', Date.now())}}
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