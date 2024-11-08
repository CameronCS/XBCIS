import React, { useState } from 'react';
import { View, Text, TextInput, TouchableOpacity, StyleSheet, Alert } from 'react-native';
import { Ionicons } from '@expo/vector-icons';
import AsyncStorage from '@react-native-async-storage/async-storage';

export default function ComposeMessage({ user, onSendMessage }) {
  const [recipient, setRecipient] = useState('');
  const [subject, setSubject] = useState('');
  const [message, setMessage] = useState('');

  const handleSendMessage = async () => {
    if (!recipient || !subject || !message) {
      Alert.alert('Error', 'Please fill out all fields.');
      return;
    }

    const newMessage = {
      id: Date.now().toString(),
      recipient,
      subject,
      body: message,
      sender: user.username,
      timestamp: new Date().toLocaleString(),
    };

    try {
      const storedMessages = await AsyncStorage.getItem('messages');
      const messagesArray = storedMessages ? JSON.parse(storedMessages) : [];

      if (messagesArray.length >= 200) {
        Alert.alert('Storage Limit', 'Message storage is full. Please delete old messages.');
        return;
      }

      const updatedMessages = [newMessage, ...messagesArray];
      await AsyncStorage.setItem('messages', JSON.stringify(updatedMessages));

      Alert.alert('Message Sent', `To: ${recipient}\nSubject: ${subject}`);
      setRecipient('');
      setSubject('');
      setMessage('');
      onSendMessage(newMessage);
    } catch (error) {
      Alert.alert('Error', 'Failed to send message. Try again.');
    }
  };

  return (
    <View style={styles.container}>
      <Text style={styles.header}>Compose Message</Text>
      <TextInput
        style={styles.input}
        placeholder="Recipient"
        value={recipient}
        onChangeText={setRecipient}
      />
      <TextInput
        style={styles.input}
        placeholder="Subject"
        value={subject}
        onChangeText={setSubject}
      />
      <TextInput
        style={styles.messageInput}
        placeholder="Message"
        value={message}
        onChangeText={setMessage}
        multiline
      />
      <TouchableOpacity style={styles.sendButton} onPress={handleSendMessage}>
        <Ionicons name="send-outline" size={20} color="#fff" />
        <Text style={styles.sendButtonText}>Send Message</Text>
      </TouchableOpacity>
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    padding: 16,
    backgroundColor: '#f6f6f6',
  },
  header: {
    fontSize: 24,
    fontWeight: 'bold',
    marginBottom: 16,
    textAlign: 'center',
    color: '#6B4EFF',
  },
  input: {
    height: 40,
    borderColor: 'gray',
    borderWidth: 1,
    borderRadius: 8,
    paddingHorizontal: 10,
    marginBottom: 20,
    backgroundColor: '#fff',
  },
  messageInput: {
    height: 100,
    borderColor: 'gray',
    borderWidth: 1,
    borderRadius: 8,
    paddingHorizontal: 10,
    marginBottom: 20,
    backgroundColor: '#fff',
  },
  sendButton: {
    backgroundColor: '#6B4EFF',
    padding: 12,
    borderRadius: 10,
    alignItems: 'center',
    flexDirection: 'row',
    justifyContent: 'center',
  },
  sendButtonText: {
    color: '#fff',
    fontSize: 16,
    marginLeft: 10,
  },
});
