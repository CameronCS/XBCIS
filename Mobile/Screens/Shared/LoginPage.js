import React, { useState, useEffect } from 'react';
import { View, Text, TextInput, Button, StyleSheet, Image, Alert } from 'react-native';
import { useNavigation } from '@react-navigation/native';
import axios from 'axios';
import { LOGIN_URL } from './Link';

export default function LoginPage({ setLoggedIn, setIsAdmin, setUser }) {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');

  const navigation = useNavigation();

  const btnSigningClick = async () => {
    if (!username || !password) {
      Alert.alert("Error", "Username and password must not be empty");
      return;
    }

    try {
      let response = await axios.post(LOGIN_URL, { username, password })
      
      const { user, message } = response.data;
      console.log(JSON.stringify(user));
      console.log(message);
      
      if (response.status === 200) {
        // Successfully logged in
        setLoggedIn(true);
        setIsAdmin(user.isAdmin);
        setUser(user);
        if (user.is_admin === 1) {
          Alert.alert('Admin Support is not valid', 'Admin support is not set on mobile devices, please log into a desktop')
          setIsAdmin(false)
          navigation.navigate('LoginPage')
        } else {
          navigation.navigate('Handler');
        }

      } else {
        Alert.alert("Username or Password Incorrect");
      }
    } catch (err) {
      Alert.alert("Username or Password Incorrect");
    }
  }

  return (
    <View style={styles.container}>
      <Image source={require('../../Resources/logo.png')} style={styles.logo} />
      <Text style={styles.label}>Username</Text>
      <TextInput
        style={styles.input}
        value={username}
        onChangeText={setUsername}
        placeholder="Enter your username"
      />
      <Text style={styles.label}>Password</Text>
      <TextInput
        style={styles.input}
        value={password}
        onChangeText={setPassword}
        placeholder="Enter your password"
        secureTextEntry
      />
      <View style={{margin: 10}}>
        <Button title="Forgot Password?" onPress={() => { navigation.navigate('ForgotPass') }} />
      </View>
      <View style={{margin: 10}}>
        <Button title="Sign in" onPress={btnSigningClick} />
      </View>
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    backgroundColor: '#F6A1F8',
  },
  logo: {
    width: 200,
    height: 200,
    marginBottom: 20,
  },
  label: {
    fontSize: 18,
    marginVertical: 10,
  },
  input: {
    width: '80%',
    height: 40,
    borderColor: 'gray',
    borderWidth: 1,
    paddingLeft: 10,
    marginBottom: 20,
    borderRadius: 5,
  },
  progressContainer: {
    width: '80%',
    marginTop: 20,
  },
});
