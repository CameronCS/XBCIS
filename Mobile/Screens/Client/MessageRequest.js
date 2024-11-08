import React, { useState } from 'react';
import { View, Text, TextInput, TouchableOpacity, StyleSheet, Image, Alert } from 'react-native';
import axios from 'axios';
import { SEND_MESSAGE } from '../Shared/Link';

export default function MessageRequest({ currentUser }) {
  const [subject, setSubject] = useState('');
  const [body, setBody] = useState('');
  const [receiver_username, setReceiverUsername] = useState(''); // Add receiver username state

  const sendMessage = () => {
    if (!subject || !body || !receiver_username) {
      Alert.alert("Error", "Please fill in all fields.");
      return;
    }

    let sender_username = currentUser.username;
    axios.post(`${SEND_MESSAGE}`, { subject, body, receiver_username, sender_username })
      .then(response => {
        Alert.alert("Success", "Message sent successfully.");
        setSubject('');
        setBody('');
        setReceiverUsername('');
      })
      .catch(error => {
        console.error('Error sending message:', error);
        Alert.alert("Error", "Failed to send message. Please try again.");
      });
  };

  return (
    <View style={styles.container}>
      <Image source={require('../../Resources/mail-logo.webp')} style={styles.mailIcon} />
      <Text style={styles.headerText}>Compose Message</Text>

      <TextInput
        style={styles.input}
        placeholder="Receiver Username"
        value={receiver_username}
        onChangeText={setReceiverUsername}
      />
      <TextInput
        style={styles.input}
        placeholder="Subject"
        value={subject}
        onChangeText={setSubject}
      />
      <TextInput
        style={[styles.input, styles.textArea]}
        placeholder="Message Body"
        value={body}
        onChangeText={setBody}
        multiline
      />

      <TouchableOpacity style={styles.button} onPress={sendMessage}>
        <Text style={styles.buttonText}>Send Message</Text>
      </TouchableOpacity>
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    alignItems: 'center',
    justifyContent: 'center',
    backgroundColor: '#F6A1F8',
  },
  mailIcon: {
    width: 200,
    height: 200,
    marginBottom: 20,
  },
  headerText: {
    fontSize: 20,
    fontWeight: 'bold',
    marginBottom: 20,
    color: '#FFFFFF'
  },
  input: {
    width: '80%',
    padding: 10,
    borderRadius: 10,
    marginVertical: 10,
    backgroundColor: '#FFFFFF',
    borderColor: '#CCCCCC',
    borderWidth: 1,
  },
  textArea: {
    height: 100,
    textAlignVertical: 'top',
  },
  button: {
    backgroundColor: '#FF69B4',
    paddingHorizontal: 20,
    paddingVertical: 10,
    borderRadius: 20,
    marginVertical: 10,
    width: '80%',
    alignItems: 'center'
  },
  buttonText: {
    color: 'white',
    fontSize: 16,
  },
});
