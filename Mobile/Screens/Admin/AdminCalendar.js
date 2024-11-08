import React, { useState } from 'react';
import { View, Text, FlatList, Alert, TouchableOpacity, StyleSheet, Modal, TextInput, ScrollView } from 'react-native';
import DateTimePickerModal from 'react-native-modal-datetime-picker';
import { Ionicons } from '@expo/vector-icons';

export default function AdminCalendar() {
  const [events, setEvents] = useState([]);
  const [isDatePickerVisible, setDatePickerVisibility] = useState(false);
  const [isTimePickerVisible, setTimePickerVisibility] = useState(false);
  const [newEvent, setNewEvent] = useState({ title: '', date: null, time: null });
  const [isModalVisible, setModalVisible] = useState(false);

  // Show and hide date picker
  const showDatePicker = () => setDatePickerVisibility(true);
  const hideDatePicker = () => setDatePickerVisibility(false);

  // Show and hide time picker
  const showTimePicker = () => setTimePickerVisibility(true);
  const hideTimePicker = () => setTimePickerVisibility(false);

  // Handle date and time selection
  const handleDateConfirm = (date) => {
    setNewEvent({ ...newEvent, date: date.toLocaleDateString() });
    hideDatePicker();
  };

  const handleTimeConfirm = (time) => {
    setNewEvent({ ...newEvent, time: time.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' }) });
    hideTimePicker();
  };

  const handleEventInput = (text) => {
    setNewEvent({ ...newEvent, title: text });
  };

  // Add new event
  const addEvent = () => {
    if (!newEvent.title || !newEvent.date || !newEvent.time) {
      Alert.alert("Missing Details", "Please fill in all fields (title, date, time) before saving the event.");
      return;
    }

    const newEventData = { id: Date.now().toString(), ...newEvent };
    setEvents([...events, newEventData]);
    setNewEvent({ title: '', date: null, time: null });
    setModalVisible(false);
  };

  const deleteEvent = (id) => {
    Alert.alert("Confirm Deletion", "Are you sure you want to delete this event?", [
      { text: "Cancel", style: "cancel" },
      { text: "Delete", onPress: () => setEvents(events.filter(event => event.id !== id)) }
    ]);
  };

  const renderEvent = ({ item }) => (
    <View style={styles.eventContainer}>
      <Text style={styles.eventText}>{item.title}</Text>
      <Text style={styles.eventDate}>{item.date} at {item.time}</Text>
      <TouchableOpacity onPress={() => deleteEvent(item.id)} style={styles.deleteButton}>
        <Ionicons name="trash-outline" size={20} color="#fff" />
      </TouchableOpacity>
    </View>
  );

  return (
    <View style={styles.container}>
      <Text style={styles.header}>Admin Calendar Management</Text>
      
      <TouchableOpacity style={styles.addButton} onPress={() => setModalVisible(true)}>
        <Ionicons name="add-circle-outline" size={28} color="#fff" />
        <Text style={styles.addButtonText}>Add New Event</Text>
      </TouchableOpacity>

      <FlatList
        data={events}
        renderItem={renderEvent}
        keyExtractor={(item) => item.id}
        style={styles.eventList}
      />

      {/* Modal for adding a new event */}
      <Modal visible={isModalVisible} animationType="slide" transparent={true}>
        <View style={styles.modalContainer}>
          <View style={styles.modalContent}>
            <Text style={styles.modalTitle}>Add New Event</Text>
            <TextInput
              placeholder="Event Title"
              style={styles.input}
              value={newEvent.title}
              onChangeText={handleEventInput}
            />

            <TouchableOpacity style={styles.dateButton} onPress={showDatePicker}>
              <Ionicons name="calendar-outline" size={24} color="#fff" />
              <Text style={styles.buttonText}>{newEvent.date || 'Select Date'}</Text>
            </TouchableOpacity>
            <DateTimePickerModal
              isVisible={isDatePickerVisible}
              mode="date"
              onConfirm={handleDateConfirm}
              onCancel={hideDatePicker}
            />

            <TouchableOpacity style={styles.dateButton} onPress={showTimePicker}>
              <Ionicons name="time-outline" size={24} color="#fff" />
              <Text style={styles.buttonText}>{newEvent.time || 'Select Time'}</Text>
            </TouchableOpacity>
            <DateTimePickerModal
              isVisible={isTimePickerVisible}
              mode="time"
              onConfirm={handleTimeConfirm}
              onCancel={hideTimePicker}
            />

            <TouchableOpacity style={styles.saveButton} onPress={addEvent}>
              <Text style={styles.saveButtonText}>Save Event</Text>
            </TouchableOpacity>
            <TouchableOpacity style={styles.cancelButton} onPress={() => setModalVisible(false)}>
              <Text style={styles.cancelButtonText}>Cancel</Text>
            </TouchableOpacity>
          </View>
        </View>
      </Modal>
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    padding: 16,
    backgroundColor: '#F6A1F8',
  },
  header: {
    fontSize: 26,
    fontWeight: 'bold',
    marginBottom: 20,
    textAlign: 'center',
    color: '#fff',
  },
  eventList: {
    marginTop: 16,
  },
  eventContainer: {
    marginBottom: 15,
    backgroundColor: '#6B4EFF',
    padding: 15,
    borderRadius: 10,
    elevation: 3,
    position: 'relative',
  },
  eventText: {
    fontSize: 18,
    color: '#fff',
    fontWeight: 'bold',
  },
  eventDate: {
    fontSize: 16,
    color: '#eee',
    marginTop: 5,
  },
  deleteButton: {
    position: 'absolute',
    top: 10,
    right: 10,
    backgroundColor: '#ff4444',
    padding: 8,
    borderRadius: 5,
  },
  addButton: {
    backgroundColor: '#6B4EFF',
    padding: 15,
    borderRadius: 10,
    alignItems: 'center',
    flexDirection: 'row',
    justifyContent: 'center',
    marginBottom: 20,
  },
  addButtonText: {
    color: '#fff',
    fontSize: 18,
    marginLeft: 10,
  },
  modalContainer: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    backgroundColor: 'rgba(0, 0, 0, 0.7)',
  },
  modalContent: {
    backgroundColor: '#fff',
    padding: 20,
    borderRadius: 10,
    width: '80%',
    alignItems: 'center',
  },
  modalTitle: {
    fontSize: 22,
    fontWeight: 'bold',
    marginBottom: 20,
  },
  input: {
    borderColor: '#ddd',
    borderWidth: 1,
    borderRadius: 5,
    padding: 10,
    width: '100%',
    marginBottom: 20,
    fontSize: 16,
  },
  dateButton: {
    backgroundColor: '#6B4EFF',
    padding: 12,
    borderRadius: 10,
    marginBottom: 20,
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'center',
    width: '100%',
  },
  buttonText: {
    color: '#fff',
    fontSize: 16,
    marginLeft: 10,
  },
  saveButton: {
    backgroundColor: '#4CAF50',
    padding: 12,
    borderRadius: 10,
    marginBottom: 10,
    width: '100%',
    alignItems: 'center',
  },
  saveButtonText: {
    color: '#fff',
    fontSize: 16,
  },
  cancelButton: {
    backgroundColor: '#FF6B6B',
    padding: 12,
    borderRadius: 10,
    width: '100%',
    alignItems: 'center',
  },
  cancelButtonText: {
    color: '#fff',
    fontSize: 16,
  },
});
