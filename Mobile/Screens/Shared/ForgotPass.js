import React, { useState, useEffect } from 'react';
import { View, Text, TextInput, Button, StyleSheet, Image, Alert } from 'react-native';
import { useNavigation } from '@react-navigation/native';
import axios from 'axios';
import { RESET_PASSWORD_URL } from './Link';

export default function ForgotPass() {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');

  const navigation = useNavigation();

  const btnResetClick = async () => {
    if (!username) {
      Alert.alert("Error", "Please enter your username");
      return;
    }
    if (!password) {
      Alert.alert("Error", "Please enter your new password");
      return;
    }
    if (!confirmPassword) {
      Alert.alert("Error", "Please confirm your new password");
      return;
    }
    
    if (password !== confirmPassword) {
        Alert.alert("Error", "Passwords do not match");
        return;
    }

    try {
        let response = await axios.put(RESET_PASSWORD_URL, { username, password })
        
        const { user, message } = response.data;
        console.log(JSON.stringify(user));
        console.log(message);
  
        if (response.status === 404) {
          Alert.alert("Error", "There was an error resetting your password please try again later")
          return;
        } else if(response.status === 500) {
          Alert.alert("Error", "There was an error resetting your password please try again later")
          return 
        } else {
          Alert.alert("Reset", "Your password was reset successfully")
          navigation.navigate('LoginPage')
        }
      } catch(err) {
        Alert.alert("Error", "There was an error resetting your password please try again later")
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
      <Text style={styles.label}>New Password</Text>
      <TextInput
        style={styles.input}
        value={password}
        onChangeText={setPassword}
        placeholder="Enter your password"
        secureTextEntry
      />
      <Text style={styles.label}>Confirm Password</Text>
      <TextInput
        style={styles.input}
        value={confirmPassword}
        onChangeText={setConfirmPassword}
        placeholder="Enter your password"
        secureTextEntry
      />
      <View style={{margin: 10}}>
        <Button title="Reset" onPress={btnResetClick} />
      </View>

      <View style={{margin: 10}}>
        <Button title="Back" onPress={() => { navigation.navigate('LoginPage') }} />
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
