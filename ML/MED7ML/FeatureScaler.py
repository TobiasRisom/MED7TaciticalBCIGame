import pandas as pd
import os
import matplotlib.pyplot as plt


# Read the CSV file into a DataFrame
def read_csv_to_dataframe(file_path):
    try:
        df = pd.read_csv(file_path)
        return df
    except FileNotFoundError:
        print(f"Error: File not found at path {file_path}")
        return None
    except Exception as e:
        print(f"An error occurred: {e}")
        return None


# Combine all data files into one large data set in a dataframe
def files_to_df():
    path = 'Data/Outputs_all'
    all_files = os.listdir(path)
    print(all_files)
    full_df = pd.DataFrame()
    for file in all_files:
        file_path = os.path.join(path, file)
        file_df = read_csv_to_dataframe(file_path)
        full_df = pd.concat([full_df, file_df], ignore_index=True)
    return full_df


# Graphs the specified column in a bar graph. Saves the graph under /Figures
def graph_it(df, column):
    series = pd.Series(df[column])  # Turns the column into a pandas series
    frequency = series.value_counts().sort_index()  # Gets the frequency for each distinct value in the series

    # Creates and adjusts a bar graph and saves it to the Figures folder
    plt.figure(figsize=(10, 6))
    plt.bar(frequency.index, frequency.values, color='green', alpha=0.75)
    plt.ylabel('Frequency')
    plt.title(column)
    plt.savefig('Figures/' + column + '.png')
    plt.show()


# Turns the separate files into one large dataframe
p_data_df = files_to_df()
p_data_df.info()
p_data_df.to_csv('Data/all_data.csv') # Saves all data into one csv file

# Loops over all the wanted columns and graphs them
graph_columns = ['Success', 'Goblin Death', 'Player Hurt', 'Player Attacking', 'Player Moving', 'Type']
for i in graph_columns:
    graph_it(p_data_df, i)
