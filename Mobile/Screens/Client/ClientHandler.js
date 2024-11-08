import { View, Text, Image, StyleSheet } from 'react-native'
import React from 'react'
import { createDrawerNavigator } from '@react-navigation/drawer'
import HomePage from './HomePage'
import Inbox from './Inbox'
import MessageRequest from './MessageRequest'
import Resources from './Resources'
import Gallery from './Gallery'
import Profile from './Profile'
import Calendar from './Calendar'
import NewsLetter from './NewsLetter'
export default function ClientHandler({user, setLoggedIn}) {
    const currentUser = user
    const Drawer = createDrawerNavigator();

    return (
        <Drawer.Navigator>
            {/* <Drawer.Screen 
                name='Home' 
                options={{
                    drawerIcon: () => <Image source={require("../../Resources/ibIcon.png")} style={styles.Images}/>
                }}
            >
                {props => <HomePage {...props} currentUser={currentUser}/>}
            </Drawer.Screen> */}

            <Drawer.Screen 
                name='Inbox' 
                options={{
                    drawerIcon: () => <Image source={require("../../Resources/ibIcon.png")} style={styles.Images}/>
                }}
            >
                {props => <Inbox {...props} currentUser={currentUser}/>}
            </Drawer.Screen>

            <Drawer.Screen 
                name='Message Request' 
                options={{
                    drawerIcon: () => <Image source={require("../../Resources/mrIcon.png")} style={styles.Images}/>
                }}
            >
                {props => <MessageRequest {...props} currentUser={currentUser}/>}
            </Drawer.Screen>

            <Drawer.Screen 
                name='Resources' 
                options={{
                    drawerIcon: () => <Image source={require("../../Resources/rcIcon.png")} style={styles.Images}/>
                }}
            >
                {props => <Resources {...props} currentUser={currentUser}/>}
            </Drawer.Screen>

            <Drawer.Screen 
                name='Calendar' 
                options={{
                    drawerIcon: () => <Image source={require("../../Resources/cdIcon.png")} style={styles.Images}/>
                }}
            >
                {props => <Calendar {...props} currentUser={currentUser}/>}
            </Drawer.Screen>

            <Drawer.Screen 
                name='Gallery' 
                options={{
                    drawerIcon: () => <Image source={require("../../Resources/glIcon.png")} style={styles.Images}/>
                }}   
            >
                {props => <Gallery {...props} currentUser={currentUser}/>}
            </Drawer.Screen>
            
            <Drawer.Screen 
                name='NewsLetters' 
                options={{
                    drawerIcon: () => <Image source={require("../../Resources/nlIcon.png")} style={styles.Images}/>
                }}
            >
                {props => <NewsLetter {...props} currentUser={currentUser}/>}
            </Drawer.Screen>

            <Drawer.Screen 
                name='Profile' 
                options={{
                    drawerIcon: () => <Image source={require("../../Resources/pfIcon.png")} style={styles.Images}/>
                }}       
            >
                {props => <Profile {...props} currentUser={currentUser} setLoggedIn={setLoggedIn}/>}
            </Drawer.Screen>
        </Drawer.Navigator>
    )
}

const styles = StyleSheet.create({
    Images: {
        width: 20,
        height: 20
    }
})
