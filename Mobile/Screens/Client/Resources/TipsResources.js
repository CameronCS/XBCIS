import React, { useEffect, useState } from 'react';
import { View, Text, StyleSheet, FlatList, TouchableOpacity, Modal } from 'react-native';
import axios from 'axios';
import { GET_TIPSRESOURCE } from '../../Shared/Link';

export default function TipsResources() {
  const [tips, setTips] = useState([]);
  const [selectedTip, setSelectedTip] = useState(null);
  const [modalVisible, setModalVisible] = useState(false);

  useEffect(() => {
    axios.get(GET_TIPSRESOURCE)
      .then(response => {        
        setTips(response.data.resources)
      })
      .catch(error => console.error(error));
  }, []);

  const openModal = (tip) => {
    setSelectedTip(tip);
    setModalVisible(true);
  };

  const closeModal = () => {
    setModalVisible(false);
    setSelectedTip(null);
  };

  return (
    <View style={styles.container}>
      <FlatList
        data={tips}
        keyExtractor={item => item.id.toString()}
        renderItem={({ item }) => (
          <TouchableOpacity style={styles.tipItem} onPress={() => openModal(item)}>
            <Text style={styles.tipTitle}>{item.title}</Text>
          </TouchableOpacity>
        )}
      />

      <Modal visible={modalVisible} animationType="slide" onRequestClose={closeModal}>
        <View style={styles.modalContainer}>
          <Text style={styles.modalTitle}>{selectedTip?.title}</Text>
          <Text style={styles.modalText}>{selectedTip?.tip_text}</Text>
          <TouchableOpacity onPress={closeModal} style={styles.closeButton}>
            <Text style={styles.closeButtonText}>Close</Text>
          </TouchableOpacity>
        </View>
      </Modal>
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    padding: 20,
    backgroundColor: '#F6A1F8',
  },
  tipItem: {
    padding: 15,
    backgroundColor: '#FFFFFF',
    marginBottom: 10,
    borderRadius: 10,
  },
  tipTitle: {
    fontSize: 18,
    fontWeight: 'bold',
  },
  modalContainer: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    padding: 20,
    backgroundColor: 'rgba(0, 0, 0, 0.5)',
  },
  modalTitle: {
    fontSize: 24,
    fontWeight: 'bold',
    marginBottom: 15,
  },
  modalText: {
    fontSize: 16,
    marginBottom: 20,
  },
  closeButton: {
    padding: 10,
    backgroundColor: '#FF69B4',
    borderRadius: 10,
  },
  closeButtonText: {
    color: '#FFFFFF',
    fontSize: 16,
  },
});
