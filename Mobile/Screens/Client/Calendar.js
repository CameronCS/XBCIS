import React, { useEffect, useState } from 'react';
import { View, Text, StyleSheet, ScrollView, TouchableOpacity, Modal, Button } from 'react-native';
import axios from 'axios';
import { GET_CALENDAR } from '../Shared/Link'; // Assuming GET_CALENDAR is the correct API endpoint

const generateDays = (year) => {
  const months = [
    { name: "January", days: 31 },
    { name: "February", days: year % 4 === 0 ? 29 : 28 },
    { name: "March", days: 31 },
    { name: "April", days: 30 },
    { name: "May", days: 31 },
    { name: "June", days: 30 },
    { name: "July", days: 31 },
    { name: "August", days: 31 },
    { name: "September", days: 30 },
    { name: "October", days: 31 },
    { name: "November", days: 30 },
    { name: "December", days: 31 }
  ];

  let events = {};
  months.forEach(month => {
    events[month.name] = Array.from({ length: month.days }, (_, i) => ({
      date: (i + 1).toString(),
      events: []
    }));
  });

  return events;
};

export default function Calendar() {
  const [currentMonth, setCurrentMonth] = useState('January');
  const [selectedDay, setSelectedDay] = useState(null);
  const [modalVisible, setModalVisible] = useState(false);
  const [events, setEvents] = useState(generateDays(2024));

  useEffect(() => {
    const fetchEvents = async () => {
      try {
        const response = await axios.get(GET_CALENDAR);
        const { results } = response.data; // Assuming the API returns an array of events

        // Parse and distribute events by month and day
        const updatedEvents = generateDays(2024);
        results.forEach(event => {
          const eventDate = new Date(event.event_date);
          const monthName = eventDate.toLocaleString('default', { month: 'long' });
          const day = eventDate.getDate().toString();

          if (updatedEvents[monthName] && updatedEvents[monthName][day - 1]) {
            updatedEvents[monthName][day - 1].events.push(event.title);
          }
        });

        setEvents(updatedEvents);
      } catch (error) {
        console.error("Failed to fetch calendar events", error);
      }
    };

    fetchEvents();
  }, []);

  const handleDayPress = (day) => {
    setSelectedDay(events[currentMonth].find(d => d.date === day.date) || { date: day.date, events: ["No events"] });
  };

  const renderDay = (day) => (
    <TouchableOpacity key={day.date} style={styles.day} onPress={() => handleDayPress(day)}>
      <Text style={styles.date}>{day.date}</Text>
      {day.events.length > 0 && <Text style={styles.tag}>{day.events.join(', ')}</Text>}
    </TouchableOpacity>
  );

  return (
    <ScrollView style={styles.container}>
      <View style={styles.header}>
        <TouchableOpacity onPress={() => setModalVisible(true)}>
          <Text style={styles.headerText}>{currentMonth} 2024</Text>
        </TouchableOpacity>
        <Modal
          transparent={true}
          visible={modalVisible}
          onRequestClose={() => setModalVisible(false)}
        >
          <View style={styles.modalBackground}>
            <View style={styles.modalContainer}>
              {Object.keys(events).map((month) => (
                <TouchableOpacity key={month} onPress={() => { setCurrentMonth(month); setModalVisible(false); }}>
                  <Text style={styles.modalItem}>{month}</Text>
                </TouchableOpacity>
              ))}
            </View>
          </View>
        </Modal>
      </View>
      <View style={styles.month}>
        {events[currentMonth].map(renderDay)}
      </View>
      <Modal
        animationType="slide"
        transparent={true}
        visible={!!selectedDay}
        onRequestClose={() => setSelectedDay(null)}
      >
        <View style={styles.modalView}>
          <Text style={styles.modalText}>Events on {selectedDay?.date}:</Text>
          {selectedDay?.events.map((event, index) => (
            <Text key={index} style={styles.eventText}>{event}</Text>
          ))}
          <Button title="Close" onPress={() => setSelectedDay(null)} />
        </View>
      </Modal>
    </ScrollView>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#F6A1F8',
  },
  month: {
    flexDirection: 'row',
    flexWrap: 'wrap',
    justifyContent: 'flex-start'
  },
  day: {
    width: '14%',
    minHeight: 80,
    backgroundColor: '#EAEAEA',
    justifyContent: 'center',
    alignItems: 'center',
    marginVertical: 2,
    marginHorizontal: 0.5
  },
  date: {
    fontSize: 16,
    fontWeight: 'bold',
    color: '#333'
  },
  tag: {
    fontSize: 12,
    color: '#666'
  },
  header: {
    padding: 20,
    backgroundColor: '#A17AFE',
    alignItems: 'center'
  },
  headerText: {
    fontSize: 22,
    color: 'white',
    fontWeight: 'bold'
  },
  modalBackground: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    backgroundColor: 'rgba(0,0,0,0.5)'
  },
  modalContainer: {
    backgroundColor: 'white',
    padding: 20,
    borderRadius: 10,
    width: '80%',
    alignItems: 'center'
  },
  modalItem: {
    fontSize: 18,
    paddingVertical: 10
  },
  modalView: {
    margin: 20,
    backgroundColor: "white",
    borderRadius: 20,
    padding: 35,
    alignItems: "center",
    shadowColor: "#000",
    shadowOffset: {
      width: 0,
      height: 2
    },
    shadowOpacity: 0.25,
    shadowRadius: 3.84,
    elevation: 5
  },
  modalText: {
    marginBottom: 15,
    textAlign: "center"
  },
  eventText: {
    color: '#333',
    fontSize: 16
  }
});
